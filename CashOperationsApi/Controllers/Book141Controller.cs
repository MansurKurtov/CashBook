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

namespace CashOperationsApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Book141Controller : ControllerBase
    {
        /// <summary>
        ///  
        /// </summary>
        private readonly IBook141Service _book141Service;
        private ILogger<Book141Controller> _logger;

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
        /// <param name="book141Service"></param>
        public Book141Controller(IBook141Service book141Service, ILogger<Book141Controller> logger)
        {
            _book141Service = book141Service;
            _logger = logger;
        }

        /// <summary>
        /// Get by Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book141View)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            try
            {
                return _book141Service.GetByDate(CompanyId, UserId, date);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book141Api/GetByDate", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// Set first saldo informations for Book141
        /// </summary>
        /// <param name="date"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book141View)]
        public ResponseCoreData SetFirstSaldo(DateTime date, int saldoBeginCount, double saldoBeginSumma)
        {
            try
            {
                return _book141Service.SetFirstSaldo(CompanyId, Permissions, date, saldoBeginCount, saldoBeginSumma);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book141Api/SetFirstSaldo", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book141View)]
        public ResponseCoreData GetChiefAccountantName()
        {
            try
            {
                return _book141Service.GetChiefAccountantName(CompanyId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book141Api/GetChiefAccountantName", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book141View)]
        public ResponseCoreData GetBankName()
        {
            try
            {
                return _book141Service.GetBankName(CompanyId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book141Api/GetBankName", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// ExportToExcel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book141View)]
        public FileContentResult ExportToExcel([FromBody] List<ExcelModelForBook141> model)
        {
            try
            {
                var file = _book141Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
                var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book141";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book141.xlsx");
                System.IO.File.WriteAllBytes(path, file);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
            catch(Exception ex)
            {
                _logger.LogError("Book141Api/ExportToExcel", ex.Message);
                return null;
            }
        }
    }
}