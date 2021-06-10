using AuthService.Helpers;
using AuthService.Interfaces;
using AuthService.Models;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Models.Auth;
using Entitys.QueryModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class TokenService : EntityRepositoryCore<AuthUserRTokens>, ITokenService
    {
        private IDbContext _context;
        private readonly DataContext _dBContext;

        public TokenService(IDbContext context, DataContext dBContext) : base(context)
        {
            _context = context;
            _dBContext = dBContext;
        }
        public AuthUserRTokens InsertToken(AuthUserRTokens model)
        {

            var userTokens = Find(x => x.UserId == model.UserId)
                .OrderByDescending(x => x.Id).ToList();

            int n = 1;
            foreach (var token in userTokens)
            {
                if (AuthParams.MAX_DEVICE_COUNT <= n++)
                {
                    _dBContext.AuthUserRTokens.Remove(token);
                    _dBContext.SaveChanges();
                }
            }

            _dBContext.AuthUserRTokens.Add(model);
            _dBContext.SaveChanges();
            return model;
        }

        public void UpdateToken(AuthUserRTokens model)
        {
            _dBContext.AuthUserRTokens.Update(model);
            _dBContext.SaveChanges();
        }

        public ResponseCoreData DeleteToken(string token)
        {
            var model = Find(x => x.RefreshToken == token).SingleOrDefault();
            if (model == null)
            {
                return new ResponseCoreData(ResponseStatusCode.NotFound);
            }

            try
            {
                Delete(model.Id);
            }
            catch (Exception err)
            {
                return err;
            }

            return true;
        }

        public AuthUserRTokens FindToken(string refreshtoken)
        {
            var UserToken = _dBContext.AuthUserRTokens.Where(x => x.RefreshToken == refreshtoken).ToList().FirstOrDefault();
            return UserToken;
        }

        public ResponseCoreData DoPassword(LoginModel model)
        {
            if (model == null)
            {
                return new ResponseCoreData(ResponseStatusCode.BadRequest);
            }
            var Users = _context.DataContext.Set<AuthUsers>().Distinct().Where(x => x.UserName == model.login && x.Active).ToList();
            var user = Users.FirstOrDefault();
            
            //var tokenInfo = _context.DataContext.Set<AuthUserRTokens>();
            //var userRefTokenInfo = tokenInfo.OrderBy(o=>o.Id).LastOrDefault(f => f.UserId == user.Id);
            //if (userRefTokenInfo!=null)
            //{
            //    var min = DateTime.Now.Subtract(userRefTokenInfo.Updated).TotalMinutes;
            //    if (min <= AuthParams.EXPIRE_MINUTES_REFRESH)
            //    {
            //        return new ResponseCoreData("User faol!", ResponseStatusCode.BadRequest);
            //    }
            //}


            if (user == null)
            {
                return new ResponseCoreData("Неверный логин !", ResponseStatusCode.BadRequest);
            }

            if (!user.Active) return new ResponseCoreData
           ("Пользователь не активен !");

            if (user.Password != CryptoPasword.GetHashSalted(model.password, user.Salt))
            {
                return new ResponseCoreData("Неверный пароль !", ResponseStatusCode.BadRequest);
            }

            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

            var token = new AuthUserRTokens
            {
                ClientId = model.clientId,
                RefreshToken = refresh_token,
                UserId = user.Id,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };

            try
            {
                this.InsertToken(token);
            }
            catch (Exception err)
            {
                return new ResponseCoreData(err);
            }
            var userToken = FindToken(refresh_token);
            var data = GetJwt(userToken, refresh_token);

            return new ResponseCoreData { Result = data };
        }

        public ResponseCoreData DoRefreshToken(string refreshtoken)
        {
            if (refreshtoken == null)
            {
                return new ResponseCoreData(ResponseStatusCode.BadRequest);
            }

            var usertoken = this.FindToken(refreshtoken);

            if (usertoken == null)
            {
                return new ResponseCoreData(ResponseStatusCode.BadRequest);
            }

            var min = DateTime.Now.Subtract(usertoken.Updated).TotalMinutes;

            if (min > AuthParams.EXPIRE_MINUTES_REFRESH)
            {
                return new ResponseCoreData("Refresh token muddati tugagan!", ResponseStatusCode.Unauthorized);
            }

            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

            usertoken.RefreshToken = refresh_token;
            usertoken.Updated = DateTime.Now;

            try
            {
                var data = GetJwt(usertoken, refresh_token);
                this.Update(usertoken);
                return new ResponseCoreData { Result = data };
            }
            catch (Exception err)
            {
                return new ResponseCoreData(err);
            }
        }

        private TokenModel GetJwt(AuthUserRTokens usertoken, string refreshToken)
        {
            var now = DateTime.UtcNow;

            var model = GetUserPermissions(usertoken);
            var mainpermissions = "";
            var allpermissions = "";


            if (model.mainpermissions != null)
            {
                var permission_list = model.mainpermissions.Split(",").Distinct().ToList();
                mainpermissions = string.Join(",", permission_list);
            }

            if (model.allpermissions != null)
            {
                var permission_list = model.allpermissions.Split(",").Distinct().ToList();
                allpermissions = string.Join(",", permission_list);
            }


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usertoken.ClientId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                new Claim("UserName", usertoken.UserModel.UserName),
                new Claim("UserId", usertoken.UserId.ToString()),
                new Claim("OrgId", usertoken.UserModel.OrgId.ToString()),
                new Claim("StructureId", usertoken.UserModel.StructureId.ToString()),
                new Claim("Permissions", allpermissions),
                new Claim("Active", usertoken.UserModel.Active.ToString()),
                new Claim("FirstName", usertoken.UserModel.FirstName),
                new Claim("LastName", usertoken.UserModel.LastName),
                new Claim("MiddleName", usertoken.UserModel.MiddleName),
                new Claim("CashierTypeId", usertoken.UserModel.CashierTypeId.ToString()),
                new Claim("CashierType", GetCashierById(usertoken.UserModel.CashierTypeId)),
            };



            var symmetricKeyAsBase64 = AuthParams.PARAM_SECRET_KEY;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                issuer: AuthParams.PARAM_ISS,
                audience: AuthParams.PARAM_AUD,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(AuthParams.EXPIRE_MINUTES)),

                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            int exp_time = (int)TimeSpan.FromMinutes(AuthParams.EXPIRE_MINUTES).TotalSeconds;

            var token = new TokenModel
            {
                access_token = encodedJwt,
                expires_in = exp_time,
                refresh_token = refreshToken,
                permission = allpermissions
            };

            return token;
        }

        private string GetCashierById(int? cashierTypeId)
        {
            if (cashierTypeId == null)
                return string.Empty;

            var cashierTypes = _dBContext.CashierTypes.Where(f => f.Id == cashierTypeId).ToList();
            var cashierType = cashierTypes.FirstOrDefault();
            if (cashierType == null)
                return string.Empty;

            return cashierType.Name;
        }

        [Obsolete]
        private UserPermissionsQueryModel GetUserPermissions(AuthUserRTokens UserToken)
        {
            string userAllPermissions = "";
            string userMainPermissions = "";
            var result = new UserPermissionsQueryModel();
            try
            {
                string sql = $"select distinct p.\"permission_code\" , p.\"related_uielement_codes\", p.\"related_permission_codes\"" +
                "from \"auth_user_permissions\" rp inner join \"auth_permissions\" p on rp.\"permission_id\" = p.\"id\"" +
                "where rp.\"user_id\" = {0}";

                var list_user_permission = _context.DataContext.Query<PermissionQueryModel>().FromSql(sql, UserToken.UserId).ToList();

                foreach (var item in list_user_permission)
                {
                    userMainPermissions += item.permission_code + ",";
                    userAllPermissions += item.permission_code + ",";
                    if (item.related_permission_codes != null && item.related_permission_codes?.Length > 0)
                    {
                        userAllPermissions += item.related_permission_codes + ",";
                    }
                }
            }
            catch (Exception err)
            {
                return new UserPermissionsQueryModel();
            }

            result.allpermissions = userAllPermissions;
            result.mainpermissions = userMainPermissions;
            return result;
        }
    }
}
