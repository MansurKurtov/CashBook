using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CashOperationsApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Book120Controller : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IBook120Service _book120Service;
        private ILogger<Book120Controller> _logger;


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
        /// <param name="book120Service"></param>
        public Book120Controller(IBook120Service book120Service, ILogger<Book120Controller> logger)
        {
            _book120Service = book120Service;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book120View)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            try
            {
                return _book120Service.GetByDate(CompanyId, UserId, date);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book120Api/GetByDate", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// Set first saldo informations for Book120
        /// </summary>
        /// <param name="date"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book120View)]
        public ResponseCoreData SetFirstSaldo(DateTime date, int saldoBeginCount, double saldoBeginSumma)
        {
            try
            {
                return _book120Service.SetFirstSaldo(CompanyId, Permissions, date, saldoBeginCount, saldoBeginSumma);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book120Api/SetFirstSaldo", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book120View)]
        public ResponseCoreData GetChiefAccountantName()
        {
            try
            {
                return _book120Service.GetChiefAccountantName(CompanyId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book120Api/SetFirstSaldo", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book120View)]
        public ResponseCoreData GetBankName()
        {
            try
            {
                return _book120Service.GetBankNameById(CompanyId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book120Api/GetBankName", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// Export to Excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book120View)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<ExcelModelForBook141> model)
        {
            try
            {
                var file = _book120Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
                var fileName = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}book120";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book120.xlsx");
                System.IO.File.WriteAllBytes(path, file);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError("Book120Api/ExportToExcel", ex.Message);
                return null;
            }
        }
    }
}