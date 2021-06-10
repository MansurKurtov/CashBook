using AdminService.Helpers;
using AdminService.Interfaces;
using AuthService.Enums;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Models;
using Entitys.Models.Auth;
using Entitys.Models.Enums;
using Entitys.PostModels;
using Entitys.ViewModels.Auth;
using Entitys.ViewModels.CashOperation;
using Microsoft.EntityFrameworkCore;

using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdminService.Services
{
    /// <summary>
    /// AuthUserService for AdminApi
    /// </summary>
    public class AuthUsersService : EntityRepositoryCore<AuthUsers>, IAuthUsersService
    {
        private readonly DataContext _context;

        /// <summary>
        /// Auth User Service Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dBcontext"></param>
        public AuthUsersService(IDbContext context, DataContext dBcontext) : base(context)
        {
            _context = dBcontext;
        }

        /// <summary>
        /// Gets all users list
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetAllUsers(string permissions, int bankCode)
        {
            try
            {
                List<AuthUsers> users;

                if (permissions.Contains(Permission.CashBookSuperAdmin))
                    users = _context.AuthUsers.Where(w => w.IsAdmin == true && w.IsMain == false).ToList();

                else users = _context.AuthUsers.Where(w => w.IsAdmin == false && w.OrgId == bankCode).ToList();

                var result = users.Select(toModel).ToList();

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// This method olny for super admin, for getting simple users list
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public ResponseCoreData GetUsersOnly(string permissions, int bankCode)
        {
            try
            {
                List<AuthUsers> users;

                if (!permissions.Contains(Permission.CashBookSuperAdmin))
                    return new ResponseCoreData("У вас нет доступа для получения информации о пользователях этого типа", ResponseStatusCode.BadRequest);

                users = _context.AuthUsers.Where(w => w.IsAdmin == false && w.OrgId == bankCode).ToList();
                var result = users.Select(toModel).ToList();

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseCoreData GetUserById(int userId, string permissions, int bankCode)
        {
            try
            {
                var user = _context.AuthUsers.Where(f => f.Id == userId).ToList().FirstOrDefault();
                if (user == null)
                    return new ResponseCoreData("User not found", ResponseStatusCode.BadRequest);

                if (user.OrgId != bankCode)
                    return new ResponseCoreData("У вас нет доступа для получения информации о пользователе другого банка!", ResponseStatusCode.BadRequest);

                if (user.IsAdmin && !permissions.Contains(Permission.CashBookSuperAdmin))
                    return new ResponseCoreData("У вас нет доступа для получения информации о пользователях этого типа!",
                        ResponseStatusCode.BadRequest);

                var result = toModel(user);
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        public ResponseCoreData ChangePassword(int id, string password, string newpassword, int bankCode)
        {
            var user = _context.AuthUsers.Where(f=>f.Id==id).ToList().FirstOrDefault();
            if (user != null)
            {
                if (CheckPassword(user.Password, user.Salt, password))
                {
                    if (user.OrgId != bankCode)
                        return new ResponseCoreData("Вы не можете изменить пароль пользователя другого банка!", ResponseStatusCode.BadRequest);

                    try
                    {
                        var hash = CryptoHelper.CreateHashSalted(newpassword);
                        user.Password = hash.Hash;
                        user.Salt = hash.Salt;

                        this.Update(user);
                        return true;
                    }
                    catch (Exception err)
                    {
                        return new ResponseCoreData(err);
                    }
                }
                else
                    return new ResponseCoreData("Старый пароль не совпадает", ResponseStatusCode.ErrorInBody);
            }
            else
                return new ResponseCoreData("Не найден пользователь", ResponseStatusCode.ErrorInBody);
        }


        /// <summary>
        /// Adds new User
        /// </summary>
        /// <param name="model"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public ResponseCoreData UserInsert(UserPostModel model, int OrgId, string permissions)
        {
            if (model == null)
                return new ResponseCoreData("Пустой модель", ResponseStatusCode.ErrorInBody);

            if (model.Login == null || model.Password == null)
                return new ResponseCoreData("Не заполнен логин или пароль", ResponseStatusCode.ErrorInBody);

            var item = this.Find(x => x.UserName == model.Login).ToList().FirstOrDefault();

            if (item == null)
            {
                if (model.IsAdmin && !permissions.Contains(Permission.CashBookSuperAdmin))
                    return new ResponseCoreData("У вас нет доступа для создания администратора, только супер администратор может создавать администраторов!",
                        ResponseStatusCode.ErrorInBody);
                try
                {
                    var hash = CryptoHelper.CreateHashSalted(model.Password);

                    var user = toEntity(model);
                    user.Salt = hash.Salt;
                    user.Password = hash.Hash;

                    if (permissions.Contains(Permission.CashBookSuperAdmin))
                        user.OrgId = model.BankId;
                    else
                        user.OrgId = OrgId;

                    user.IsAdmin = model.IsAdmin;
                    user.IsMain = false;
                    user.Active = model.Active;
                    user.StructureId = 1;

                    this.Add(user);
                    var users = this.Find(x => x.UserName == model.Login).ToList().FirstOrDefault();
                    switch (user.CashierTypeId)
                    {
                        case 1:
                            model.PermissionIds.Add(GetPermissionId(Permission.CanChangeDayStatus));
                            model.PermissionIds.Add(GetPermissionId(Permission.Book171View));
                            model.PermissionIds.Add(GetPermissionId(Permission.CashBookProcedureRun));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal111View));
                            model.PermissionIds.Add(GetPermissionId(Permission.Book141View));
                            model.PermissionIds.Add(GetPermissionId(Permission.Book155Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.Book141AView));
                            model.PermissionIds.Add(GetPermissionId(Permission.Book121View));
                            break;
                        case 2:
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal109ValView));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal109WorthView));
                            model.PermissionIds.Add(GetPermissionId(Permission.Book155Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal110ValView));
                            break;
                        case 3:
                            model.PermissionIds.Add(GetPermissionId(Permission.Book155Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal110WorthView));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal110View));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal110ValView));
                            break;
                        case 4:
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal15Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.CollectorView));
                            model.PermissionIds.Add(GetPermissionId(Permission.CollectorEdit));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal16Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.Book155Edit));
                            break;

                        case 5:
                            model.PermissionIds.Add(GetPermissionId(Permission.Book175Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal176Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.Set16Aceeptance));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal16Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.Journal15Edit));
                            model.PermissionIds.Add(GetPermissionId(Permission.CounterCashierEdit));
                            break;
                    }

                    model.PermissionIds = model.PermissionIds.Distinct().ToList();
                    model.PermissionIds = model.PermissionIds.Where(w => w > 0).ToList();
                    if (user.IsAdmin && !model.PermissionIds.Contains(GetAdminPermissionId()))
                    {
                        var adminPermissionId = GetAdminPermissionId();
                        model.PermissionIds.Add(adminPermissionId);
                    }

                    _context.SaveChanges();
                    model.PermissionIds.ForEach(permission =>
                    {
                        var userPermission = new AuthUserPermissions();
                        userPermission.UserId = users.Id;
                        userPermission.PermissionId = permission;

                        //var existsItem = _context.AuthUserPermissions.Where(w => w.Id == userPermission.Id).ToList().FirstOrDefault();                        
                        _context.Entry<AuthUserPermissions>(userPermission).State = EntityState.Added;
                        _context.AuthUserPermissions.Add(userPermission);
                        _context.SaveChanges();

                        _context.Entry<AuthUserPermissions>(userPermission).State = EntityState.Detached;
                    });
                    if (user.IsAdmin)
                    {
                        SetWorkingDate(user.OrgId, user.Id, DateTime.Today);
                    }
                    var insertUserResult = ConvertToInsertUserResult(user);
                    return new ResponseCoreData(insertUserResult, ResponseStatusCode.OK);
                }
                catch (Exception err)
                {
                    return err;
                }
            }
            else
                return new ResponseCoreData("Существует такой пользователь");
        }

        private void SetWorkingDate(int? bankCode, int userId, DateTime date)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SET_WORKING_DATE";

                var dateParam = new OracleParameter("SANA", OracleDbType.Date, ParameterDirection.Input);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);

                var bankCodeParam = new OracleParameter("BANKKOD", OracleDbType.Int32, ParameterDirection.Input);
                bankCodeParam.Value = bankCode;
                cmd.Parameters.Add(bankCodeParam);

                var userIdParam = new OracleParameter("USERID", OracleDbType.Int32, ParameterDirection.Input);
                userIdParam.Value = userId;
                cmd.Parameters.Add(userIdParam);

                var rowsCountParam = new OracleParameter("RESULT_ROWS", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(rowsCountParam);

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                DataTable budt = new DataTable();
                while (reader.Read())
                {
                    budt.Load(reader);
                }
                var insertedRowsCount = int.Parse(rowsCountParam.Value.ToString());
                cmd.Connection.Close();
            }
        }


        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData UserUpdate(UserPostModel model, string permissions, int bankCode)
        {
            if (model == null && model.Login == null)
                return new ResponseCoreData("Не передаете нужные параметры", ResponseStatusCode.ErrorInBody);

            var user = _context.AuthUsers.Where(f=>f.Id== model.Id).ToList().FirstOrDefault();
            if (user != null)
            {
                if (user.UserName != model.Login)
                {
                    var item = this.Find(x => x.UserName == model.Login && x.Id != user.Id).ToList();
                    if (item.Count > 0) return new ResponseCoreData("Существует пользователь с таким названием", ResponseStatusCode.ErrorInBody);
                }

                try
                {
                    if (model.IsAdmin && !permissions.Contains(Permission.CashBookSuperAdmin))
                        return new ResponseCoreData("У вас нет доступа для изменения информации этого пользователя!",
                            ResponseStatusCode.ErrorInBody);

                    if (!permissions.Contains(Permission.CashBookSuperAdmin) && model.BankId != bankCode)
                        return new ResponseCoreData("Вы не можете изменить информацию о пользователях другого банка!", ResponseStatusCode.BadRequest);

                    var entity = toEntity(model);

                    if (permissions.Contains(Permission.CashBookSuperAdmin))
                        entity.OrgId = model.BankId;
                    else
                        entity.OrgId = bankCode;

                    this.Update(entity);

                    var userOldPermissions = _context.AuthUserPermissions.Where(w => w.UserId == entity.Id).ToList();
                    _context.AuthUserPermissions.RemoveRange(userOldPermissions);

                    model.PermissionIds = model.PermissionIds.Where(w => w > 0).ToList();

                    if (user.IsAdmin)
                    {
                        var adminPermissionId = GetAdminPermissionId();
                        if (!model.PermissionIds.Contains(adminPermissionId))
                            model.PermissionIds.Add(adminPermissionId);

                        var chiefAccountantPermissionId = GetChiefAccountantPermissionId();
                        if (!model.PermissionIds.Contains(chiefAccountantPermissionId))
                            model.PermissionIds.Add(chiefAccountantPermissionId);
                    }
                    _context.SaveChanges();

                    model.PermissionIds.ForEach(permission =>
                    {
                        var userPermission = new AuthUserPermissions();
                        userPermission.UserId = user.Id;
                        userPermission.PermissionId = permission;
                        _context.Entry<AuthUserPermissions>(userPermission).State = EntityState.Added;
                        _context.AuthUserPermissions.Add(userPermission);
                        _context.SaveChanges();
                        _context.Entry<AuthUserPermissions>(userPermission).State = EntityState.Detached;
                    });



                    var updateUserResult = ConvertToInsertUserResult(entity);
                    return new ResponseCoreData(updateUserResult, ResponseStatusCode.OK);
                }
                catch (Exception err)
                {
                    return new ResponseCoreData(err);
                }
            }
            else
                return new ResponseCoreData("Не найден пользователь", ResponseStatusCode.ErrorInBody);
        }

        private int GetAdminPermissionId()
        {
            var permission = _context.AuthPermissions.Where(f => f.PermissionCode.Equals(Permission.CashBookAdmin)).ToList().FirstOrDefault();
            if (permission == null)
                return -1;

            return permission.Id;
        }

        private int GetPermissionId(string permissionCode)
        {
            var permission = _context.AuthPermissions.Where(f => f.PermissionCode.Equals(permissionCode)).ToList().FirstOrDefault();
            if (permission == null)
                return -1;

            return permission.Id;
        }

        private int GetChiefAccountantPermissionId()
        {
            var permission = _context.AuthPermissions.Where(f => f.PermissionCode.Equals(Permission.Chiefaccountant)).ToList().FirstOrDefault();
            if (permission == null)
                return -1;

            return permission.Id;
        }

        /// <summary>
        /// Getting Banks list
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetBanks()
        {
            try
            {
                var banks = _context.Query<GetBankAll>().FromSql("select KOD as \"Id\", Name from table(GET_BANK_ALL)").ToList();
                return new ResponseCoreData(banks, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetBanksForSuperAdmin(string permissions)
        {
            if (!permissions.Contains(Permission.CashBookSuperAdmin))
                return new ResponseCoreData("Нет Доступ!", ResponseStatusCode.BadRequest);
            try
            {
                var banks = _context.Query<GetBankAll>().FromSql("select KOD as \"Id\", Name from table(GET_BANK_ALL_FOR_SADMIN)").ToList();
                return new ResponseCoreData(banks, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// Getting Cashier types
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetCashierTypes()
        {
            try
            {
                var entitys = _context.CashierTypes.ToList();
                var result = entitys.Select(ConvertToCashierTypeViewModel).ToList();
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        public ResponseCoreData GetBranches(DateTime date)
        {
            var parsedDate = date.ToString("dd.MM.yyyy");
            var sql = $"select * from table(GET_BANKS_CLOSE_DAY(to_date('{parsedDate}', 'DD.MM.YYYY')))";
            try
            {
                var branches = _context.Query<GetBranch>().FromSql(sql).ToList();
                var result = branches.Select(ConvertToGetBranchViewModel);
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        private GetBranchViewModel ConvertToGetBranchViewModel(GetBranch data)
        {
            var result = new GetBranchViewModel();
            result.MFO = data.Kod;
            result.Name = data.Name;
            result.IsClose = data.IsClose;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetPermissions()
        {
            try
            {
                var permissions = _context.AuthPermissions.Where(w => w.PermissionCode != Permission.CashBookAdmin && w.PermissionCode != Permission.CashBookSuperAdmin).ToList();
                var result = permissions.Select(ConvertToPermissionViewModel).ToList();
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        private UserPermissionViewModel ConvertToPermissionViewModel(AuthPermissions entity)
        {
            var result = new UserPermissionViewModel();
            result.Id = entity.Id;
            result.Name = entity.PermissionName;

            return result;
        }

        private AuthCashierTypeViewModel ConvertToCashierTypeViewModel(CashierType entity)
        {
            var result = new AuthCashierTypeViewModel();
            result.Id = entity.Id;
            result.Name = entity.Name;
            return result;
        }

        private InsertUserViewModel ConvertToInsertUserResult(AuthUsers user)
        {
            var result = new InsertUserViewModel();
            result.Id = user.Id;
            result.BankName = GetBankNameById(user.OrgId);
            result.CashierTypeName = GetCashierTypeById(user.CashierTypeId);
            result.PermissionIds = GetUserPermissionIds(user.Id);
            result.PermissionNames = GetUserPermissionNames(user.Id);

            return result;
        }

        public bool CheckPassword(string hash_password, string salt_password, string password)
        {
            if (hash_password == CryptoHelper.GetHashSalted(password, salt_password))
                return true;
            else
                return false;

        }
        private AuthUsers toEntity(UserPostModel model)
        {
            var entity = new AuthUsers();
            if (model.Id > 0) entity = _context.AuthUsers.Where(f=>f.Id==model.Id).ToList().FirstOrDefault();
            entity.UserName = model.Login;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.MiddleName = model.MiddleName;
            entity.Telefon = model.PhoneNumber;
            entity.Active = model.Active;
            entity.IsAdmin = model.IsAdmin;
            entity.CashierTypeId = model.CashierTypeId;

            return entity;
        }

        private UserViewModel toModel(AuthUsers entity)
        {
            var model = new UserViewModel();
            model.Id = entity.Id;
            model.Login = entity.UserName;
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.MiddleName = entity.MiddleName;
            model.PhoneNumber = entity.Telefon;
            model.BankId = entity.OrgId;
            model.BankName = GetBankNameById(entity.OrgId);
            model.Active = entity.Active;
            model.IsAdmin = entity.IsAdmin;
            model.CashierTypeId = entity.CashierTypeId;
            model.CashierTypeName = GetCashierTypeById(entity.CashierTypeId);
            model.PermissionIds = GetUserPermissionIds(entity.Id);
            model.PermissionNames = GetUserPermissionNames(entity.Id);

            return model;
        }

        private string GetBankNameById(int? orgId)
        {
            if (orgId == null)
                return string.Empty;

            var banks = _context.Query<GetBankAll>().FromSql($"select KOD as \"Id\", Name from table(GET_BANK_ALL) where KOD={orgId}").ToList();
            var bank = banks.FirstOrDefault();
            if (bank == null)
                return string.Empty;

            return bank.Name;
        }

        private string GetCashierTypeById(int? cashierTypeId)
        {
            if (cashierTypeId == null)
                return string.Empty;

            var cashierType = _context.CashierTypes.Where(f => f.Id == cashierTypeId).ToList().FirstOrDefault();
            if (cashierType == null)
                return string.Empty;

            return cashierType.Name;
        }

        private List<int> GetUserPermissionIds(int userId)
        {
            var result = _context.AuthUserPermissions.Where(w => w.UserId == userId).Select(s => s.PermissionId).ToList();
            return result;
        }

        private string GetUserPermissionNames(int userId)
        {
            var result = _context.AuthUserPermissions.Where(w => w.UserId == userId).Select(s => s.PermissionModel.PermissionName).ToList();
            return string.Join(", ", result);
        }
    }
}
