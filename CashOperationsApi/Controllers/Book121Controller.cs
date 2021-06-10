using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{


    /// <summary> 
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Book121Controller : ControllerBase
    {
        private readonly IBook121Service _book121Service;
        private ILogger<Book121Controller> _logger;

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
        /// <param name="book121Service"></param>
        public Book121Controller(IBook121Service book121Service, ILogger<Book121Controller> logger)
        {
            _book121Service = book121Service;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book121View)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            try
            {
                return _book121Service.GetByDate(CompanyId, UserId, date);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book121Api/GetByDate", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book121View)]
        public ResponseCoreData GetCurrencyTypes()
        {
            try
            {
                return _book121Service.GetCurrencyTypes();
            }
            catch (Exception ex)
            {
                _logger.LogError("Book121Api/GetCurrencyTypes", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="currencyCode"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book121View)]
        public ResponseCoreData SetFirstSaldo(DateTime date, int currencyCode, int saldoBeginCount, double saldoBeginSumma)
        {
            try
            {
                return _book121Service.SetFirstSaldo(CompanyId, Permissions, date, currencyCode, saldoBeginCount, saldoBeginSumma);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book121Api/SetFirstSaldo", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book121View)]
        public ResponseCoreData GetChiefAccountantName()
        {
            try
            {
                return _book121Service.GetChiefAccountantName(CompanyId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book121Api/GetChiefAccountantName", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book121View)]
        public ResponseCoreData GetBankName()
        {
            try
            {
                return _book121Service.GetBankName(CompanyId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book121Api/GetBankName", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book121View)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<Book121Excel> model)
        {
            try
            {
                var file = _book121Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
                var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book121";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book121.xlsx");
                System.IO.File.WriteAllBytes(path, file);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
            catch(Exception ex)
            {
                _logger.LogError("Book121Api/ExportToExcel", ex.Message);
                return null;
            }
        }
    }
}