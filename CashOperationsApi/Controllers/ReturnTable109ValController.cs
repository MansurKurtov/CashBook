using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReturnTable109ValController : ControllerBase
    {
        IReturnTable109ValService _returnTable109ValService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnTable109ValService"></param>
        public ReturnTable109ValController(IReturnTable109ValService returnTable109ValService)
        {
            _returnTable109ValService = returnTable109ValService;
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
        [CustomAuthorize(Permission.Journal109ValView)]
        public async Task<ResponseCoreData> GetByDate(DateTime date)
        {
            return _returnTable109ValService.GetByDate(CompanyId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal109ValView)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<ExcelModel> model)
        {
            var file = _returnTable109ValService.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName),CompanyId);
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}journal110val";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}journal110val.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
    }
}