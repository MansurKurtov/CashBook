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
    /// <summary>
    /// 
    /// </summary>    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Book171Controller : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IBook171Service _book171Service;

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
        private string Permissions
        {
            get { return User.FindFirst("Permissions")?.Value; }
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
        /// <param name="book171Service"></param>
        public Book171Controller(IBook171Service book171Service)
        {
            _book171Service = book171Service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book171View)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            return _book171Service.GetByDate(CompanyId, UserId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="accountCode"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book171View)]
        public ResponseCoreData SetFirstSaldo(DateTime date, string accountCode, int saldoBeginCount, double saldoBeginSumma)
        {
            return _book171Service.SetFirstSaldo(CompanyId, Permissions, date, accountCode, saldoBeginCount, saldoBeginSumma);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book171View)]
        public ResponseCoreData GetWorthAccounts()
        {
            return _book171Service.GetWorthAccounts(CompanyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book171View)]
        public ResponseCoreData GetChiefAccountantName()
        {
            return _book171Service.GetChiefAccountantName(CompanyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book171View)]
        public ResponseCoreData GetBankName()
        {
            return _book171Service.GetBankName(CompanyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book171View)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<Book171Excel> model)
        {
            var file = _book171Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book171";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book171.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
    }
}