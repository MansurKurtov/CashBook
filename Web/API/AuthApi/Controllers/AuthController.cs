using System;
using AdminService.Interfaces;
using AuthService.Enums;
using AuthService.Interfaces;
using AuthService.Jwt;
using AuthService.Models;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using MainsService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        private ITokenService _tokenService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IEntOrgService _orgService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IAuthUsersService _authUserService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenService"></param>
        /// <param name="orgService"></param>
        /// <param name="authUserService"></param>
        public AuthController(
            ITokenService tokenService,
            IEntOrgService orgService,
            IAuthUsersService authUserService
        )
        {
            _tokenService = tokenService;
            _orgService = orgService;
            _authUserService = authUserService;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseCoreData Login([FromBody]LoginModel login)
        {
            return _tokenService.DoPassword(login);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshtoken"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseCoreData RefreshToken(string refreshtoken)
        {
            return _tokenService.DoRefreshToken(refreshtoken);
        }



        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData Register([FromBody]RegisterModel model)
        {
            try
            {
                var registerOrg = _orgService.AddOrganization(model);

                if (registerOrg)
                    return new ResponseResult(true);

                return new ResponseResult("Ошибка при регистрации");
            }
            catch (Exception ex)
            {
                return new ResponseResult(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshtoken"></param>
        /// <returns></returns>
        [HttpDelete]
        public ResponseCoreData LogOut(string refreshtoken)
        {
            return _tokenService.DeleteToken(refreshtoken);
        }
    }
}