using AdminService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.PostModels.CashOperations;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AdminApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>    
    [ApiController]
    [Route("api/[controller]/[action]")]
    [CustomAuthorize(Permission.CashBookAdmin)]
    public class SupervisingAccountantController : ControllerBase
    {
        private readonly ISupervisingAccountantService _supervisingAccountantService;
        private int CompanyId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("OrgId")?.Value);
            }
        }

        private int UserId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("UserId")?.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supervisingAccountantService"></param>
        public SupervisingAccountantController(ISupervisingAccountantService supervisingAccountantService)
        {
            _supervisingAccountantService = supervisingAccountantService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseCoreData GetSupAccountant()
        {
            return _supervisingAccountantService.GetSupAccountant(CompanyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseCoreData Add([FromBody]SupervisingAccountantPostModel model)
        {
            return _supervisingAccountantService.Add(model, CompanyId, UserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public ResponseCoreData Update([FromBody]SupervisingAccountantPostModel model)
        {
            return _supervisingAccountantService.Update(model, CompanyId, UserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ResponseCoreData DeleteById(int Id)
        {
            return _supervisingAccountantService.DeleteById(Id);
        }
    }
}