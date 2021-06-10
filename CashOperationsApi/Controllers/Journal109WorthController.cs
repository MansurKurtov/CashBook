using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Journal109WorthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        IJournal109WorthService _journal109WorthService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="journal109WorthService"></param>
        public Journal109WorthController(IJournal109WorthService journal109WorthService)
        {
            _journal109WorthService = journal109WorthService;
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
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal109WorthView)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            return _journal109WorthService.GetByDate(CompanyId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[CustomAuthorize(Permission.Journal109WorthView)]
        public async Task<FileContentResult> ExportToExcel(List<Entitys.Models.ExcelModel> model)
        {
            var file = _journal109WorthService.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book109Worth";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book109Worth.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
    }
}