using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.ViewModels.CashOperation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Journal111Controller : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IJournal111Service _journal111Service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="journal111Service"></param>
        public Journal111Controller(IJournal111Service journal111Service)
        {
            _journal111Service = journal111Service;
        }

        private int UserId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("UserId")?.Value);
            }
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
        [CustomAuthorize(Permission.Journal111View)]
        public ResponseCoreData GetAll(DateTime date)
        {
            return _journal111Service.GetByDate(CompanyId, UserId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal111View)]
        public async Task<FileContentResult> ExportToExcel([FromBody] Journal111ExcelPostModel date)
        {
            var file = _journal111Service.ToExport(CompanyId, date, PutUserName.GetPutUser(FirstName, MiddleName, LastName));
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}journal111";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}journal111.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
    }
}