using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CashOperationsApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICommonService _commonService;

        /// <summary>
        /// 
        /// </summary>
        private int CompanyId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("OrgId")?.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commonService"></param>
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseCoreData GetChiefAccountantName(int bankId)
        {
            return _commonService.GetChiefAccountantName(bankId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseCoreData GetBankName(int bankId)
        {
            return _commonService.GetBankName(bankId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ResponseCoreData GetDeveloppersName()
        {
            var result = new List<string>();
            result.Add("Ibrohim, Mansur, Azamat, Jahongir, Odilbek, Shaxobiddin");
            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseCoreData GetSupAccountant()
        {
            return _commonService.GetSupAccountant(CompanyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseCoreData GetWorkingDate()
        {
            return _commonService.GetWorkingDate(CompanyId);
        }
    }
}