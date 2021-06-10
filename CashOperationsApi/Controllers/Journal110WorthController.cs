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
    public class Journal110WorthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        IJournal110WorthService _journal110WorthService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="journal110WorthService"></param>
        public Journal110WorthController(IJournal110WorthService journal110WorthService)
        {
            _journal110WorthService = journal110WorthService;
        }


        private string FirstName
        {
            get
            {
                return User.FindFirst("FirstName")?.Value;
            }
        }

        private string LastName
        {
            get
            {
                return User.FindFirst("LastName")?.Value;
            }
        }

        private string MiddleName
        {
            get
            {
                return User.FindFirst("MiddleName")?.Value;
            }
        }

        private int UserId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("UserId")?.Value);
            }
        }

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
        [CustomAuthorize(Permission.Journal110WorthView)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            return _journal110WorthService.GetByDate(CompanyId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal110WorthView)]
        public ResponseCoreData ReturnTable110(DateTime date)
        {
            return _journal110WorthService.GetCashValue(CompanyId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="operationId"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal110WorthView)]
        public async Task<ResponseCoreData> GetCashValue(DateTime date)
        {
            return _journal110WorthService.GetAll155CashValue(date, UserId, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal110WorthView)]
        public async Task<FileContentResult> ExportToExcel([FromBody] Journal110WorthExcelModel model)
        {
            var file = _journal110WorthService.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book110Worth";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book110Worth.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
        [HttpGet]
        [CustomAuthorize(Permission.Journal110WorthView)]
        public async Task<ResponseCoreData> GetWorthCashValue(DateTime date)
        {
            return _journal110WorthService.GetWorthCashValue(date, UserId);
        }
    }
}