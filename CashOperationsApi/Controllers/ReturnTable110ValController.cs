using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReturnTable110ValController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        IReturnTable110ValService _returnTable110ValService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnTable110ValService"></param>
        public ReturnTable110ValController(IReturnTable110ValService returnTable110ValService)
        {
            _returnTable110ValService = returnTable110ValService;
        }

        /// <summary>
        /// 
        /// </summary>
        private string FirstName
        {
            get
            {
                return User.FindFirst("FirstName")?.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string LastName
        {
            get
            {
                return User.FindFirst("LastName")?.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string MiddleName
        {
            get
            {
                return User.FindFirst("MiddleName")?.Value;
            }
        }

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
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal110ValView)]
        public async Task<ResponseCoreData> GetByDate(DateTime date)
        {
            return _returnTable110ValService.GetByDate(CompanyId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="operationId"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal110ValView)]
        public async Task<ResponseCoreData> GetCashValue(DateTime date)
        {
            return _returnTable110ValService.GetAll155CashValue(date, UserId, 3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal110ValView)]
        public async Task<FileContentResult> ExportToExcel([FromBody] Jourlan110ReturnExcelModel model)
        {
            var file = _returnTable110ValService.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}journal109val";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}journal109val.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal110ValView)]
        public async Task<ResponseCoreData> GetWorthCashValue(DateTime date)
        {
            return _returnTable110ValService.GetValCashValue(date, UserId);
        }
    }
}