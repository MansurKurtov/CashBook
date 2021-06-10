using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.ViewModels.CashOperation.ChiefaccountantTable;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChiefaccountantTableController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private int UserId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("UserId")?.Value);
            }
        }
        private int BankKod
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("OrgId")?.Value);
            }
        }

        private readonly IChiefaccountantTableService _chiefaccountantTableService;

        public ChiefaccountantTableController(IChiefaccountantTableService chiefaccountantTableService)
        {
            _chiefaccountantTableService = chiefaccountantTableService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Chiefaccountant)]
        public async Task<ResponseCoreData> GetAll()
        {
            return _chiefaccountantTableService.GetAll(BankKod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Chiefaccountant)]
        public async Task<ResponseCoreData> GetById(int Id)
        {
            return _chiefaccountantTableService.GetById(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.Chiefaccountant)]
        public async Task<ResponseCoreData> AddOrUpdate([FromBody] ChiefaccountantTablePostViewModel model)
        {
            model.BankKod = BankKod;
            model.UserId = UserId;
            return _chiefaccountantTableService.AddOrUpdate(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.Chiefaccountant)]
        public async Task<ResponseCoreData> Delete(int Id)
        {
            return _chiefaccountantTableService.DeleteById(Id);
        }

    }
}