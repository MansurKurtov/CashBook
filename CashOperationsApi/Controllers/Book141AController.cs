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
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Book141AController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IBook141AService _book141AService;
        private ILogger<Book141AController> _logger;


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
        /// <param name="book141AService"></param>
        public Book141AController(IBook141AService book141AService, ILogger<Book141AController> logger)
        {
            _book141AService = book141AService;
            _logger = logger;
        }

        /// <summary>
        /// Get by Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book141AView)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            try
            {
                return _book141AService.GeByDate(CompanyId, UserId, date);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book141AApi/GetByDate", ex.Message);
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
        [CustomAuthorize(Permission.Book141AView)]
        public ResponseCoreData SetFirstSaldo(DateTime date, int saldoBeginCount, double saldoBeginSumma)
        {
            try
            {
                return _book141AService.SetFirstSaldo(CompanyId, Permissions, date, saldoBeginCount, saldoBeginSumma);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book141AApi/SetFirstSaldo", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book141AView)]
        public ResponseCoreData GetChiefAccountantName()
        {
            try
            {
                return _book141AService.GetChiefAccountantName(CompanyId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book141AApi/GetChiefAccountantName", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book141AView)]
        public ResponseCoreData GetBankName()
        {
            try
            {
                return _book141AService.GetBankName(CompanyId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book141AApi/GetBankName", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// Export to excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book141AView)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<ExcelModelForBook141> model)
        {
            try
            {
                var file = _book141AService.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
                var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book141A";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book141A.xlsx");
                System.IO.File.WriteAllBytes(path, file);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError("Book141AApi/ExportToExcel", ex.Message);
                return null;
            }
        }

    }
}