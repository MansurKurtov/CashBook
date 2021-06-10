using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CounterCashierController : ControllerBase
    {
        ICounterCashierService _counterCashierService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counterCashierService"></param>
        public CounterCashierController(ICounterCashierService counterCashierService)
        {
            _counterCashierService = counterCashierService;
        }

        /// <summary>
        /// 
        /// </summary>
        private int BankCode
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("OrgId")?.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[CustomAuthorize(Permission.Journal176View)]
        public async Task<ResponseCoreData> GetAll()
        {
            return _counterCashierService.GetAll(BankCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        //[CustomAuthorize(Permission.Journal176View)]
        public async Task<ResponseCoreData> GetById(int Id)
        {
            return _counterCashierService.GetById(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.CounterCashierEdit)]
        public async Task<ResponseCoreData> Add(CounterCashier model)
        {
            return _counterCashierService.Add(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.CounterCashierEdit)]
        public async Task<ResponseCoreData> Update(CounterCashier model)
        {
            return _counterCashierService.Update(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.CounterCashierEdit)]
        public async Task<ResponseCoreData> Delete(int Id)
        {
            return _counterCashierService.Delete(Id);
        }
    }
}