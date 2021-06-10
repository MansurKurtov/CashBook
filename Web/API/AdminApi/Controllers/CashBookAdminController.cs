using System;
using System.Linq;
using AdminService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.PostModels;
using Entitys.PostModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdminApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CashBookAdminController : ControllerBase
    {
        private IAuthModulesService _authModulesService;
        private IAuthUsersService _userService;
        private ILogger<CashBookAdminController> _logger;        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        public CashBookAdminController(
            IAuthUsersService userService,
            ILogger<CashBookAdminController> logger, IAuthModulesService authModulesService
            )
        {
            _userService = userService;
            _logger = logger;
            _authModulesService = authModulesService;
        }

        /// <summary>
        /// 
        /// </summary>
        private int OrgId
        {
            get { return Convert.ToInt32(User.FindFirst("OrgId")?.Value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private int UserId
        {
            get { return Convert.ToInt32(User.FindFirst("UserId")?.Value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private int StructureId
        {
            get { return Convert.ToInt32(User.FindFirst("StructureId")?.Value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private string Permissions
        {
            get { return User.FindFirst("Permissions")?.Value; }
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData GetAllUsers()
        {
            return _userService.GetAllUsers(Permissions, OrgId);
        }

        /// <summary>
        /// Gets simple users collection for SuperAdmin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData GetUsersOnly()
        {
            return _userService.GetUsersOnly(Permissions, OrgId);
        }

        /// <summary>
        /// Gets User By userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData GetUserByID(int userId)
        {
            return _userService.GetUserById(userId, Permissions, OrgId);
        }

        /// <summary>
        /// User Activate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Activate"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData UserActivate(int id, bool Activate)
        {
            var entity = _userService.Find(x => x.Id == id).ToList().FirstOrDefault();
            entity.Active = Activate;
            _userService.Update(entity);
            return new ResponseCoreData(true, ResponseStatusCode.OK);
        }


        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData AddUser([FromBody]UserPostModel model)
        {
            try
            {
                return _userService.UserInsert(model, OrgId, Permissions);
            }
            catch (Exception err)
            {
                return err;
            }
        }
       
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData UpdateUser([FromBody]UserPostModel model)
        {
            try
            {
                return _userService.UserUpdate(model, Permissions, OrgId);
            }
            catch (Exception err)
            {
                return new ResponseCoreData(err);
            }
        }

        /// <summary>
        /// Delete user from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData DeleteUser(int id)
        {
            try
            {
                var user = _userService.Get(id);
                if (user == null)
                    return new ResponseCoreData("Пользователь не найден!",
                        ResponseStatusCode.ErrorInBody);

                if (user.IsAdmin && !Permissions.Contains(Permission.CashBookSuperAdmin))
                    return new ResponseCoreData("У вас нет доступа для удаления этого типа пользователя!",
                        ResponseStatusCode.ErrorInBody);

                _userService.Delete(id);
                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception err)
            {
                return new ResponseCoreData(err);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData ChangePassword(string oldPassword, string newPassword)
        {
            return _userService.ChangePassword(UserId, oldPassword, newPassword, OrgId);
        }

        /// <summary>
        /// Gets Banks collection
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData GetBanks()
        {
            return _userService.GetBanks();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData GetBanksForSuperAdmin()
        {
            return _userService.GetBanksForSuperAdmin(Permissions);
        }

        /// <summary>
        /// Gets Cashier Types collection
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData GetCashierTypes()
        {
            return _userService.GetCashierTypes();
        }

        /// <summary>
        /// Gets all user permissions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.CashBookAdmin)]
        public ResponseCoreData GetPermissions()
        {
            return _userService.GetPermissions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.CanChangeDayStatus)]
        public ResponseCoreData IsDayClosed(DateTime date)
        {
            return _authModulesService.IsDayClosed(date, OrgId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.CanChangeDayStatus)]
        public ResponseCoreData CloseDay([FromBody]OpenCloseDayPostModel model)
        {
            if (model == null || model.Date == null)
                return new ResponseCoreData(ResponseStatusCode.BadRequest);

            return _authModulesService.CloseDay(OrgId, UserId, model.Date,model.NextDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.CancelCloseDay)]
        public ResponseCoreData SetClearCloseDayAll([FromBody]OpenCloseDayPostModel model)
        {
            if (model == null || model.Date == null)
                return new ResponseCoreData(ResponseStatusCode.BadRequest);

            return _authModulesService.SetClearCloseDayAll(UserId, model.Date,model.Mfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.CanChangeDayStatus)]
        public ResponseCoreData OpenDay([FromBody]OpenCloseDayPostModel model)
        {
            if (model == null || model.Date == null)
                return new ResponseCoreData(ResponseStatusCode.BadRequest);

            return _authModulesService.OpenDay(OrgId, UserId, model.Date);
        }

        /// <summary>
        /// Get all bank's branches status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.BanksCloseDayView)]
        [CustomAuthorize(Permission.CancelCloseDay)]
        public ResponseCoreData GetBranches([FromQuery] DateTime date)
        {
            if (date == null)
                return new ResponseCoreData(ResponseStatusCode.BadRequest);

            return _userService.GetBranches(date);
        }
    }
}
