using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.ViewModels.CashOperation.Journal15;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Journal15Controller : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IJournal15Service _journal15Service;
        private ILogger<Journal15Controller> _logger;

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
        /// <param name="book15Service"></param>
        public Journal15Controller(IJournal15Service book15Service, ILogger<Journal15Controller> logger)
        {
            _journal15Service = book15Service;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="journal16Id"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal15View)]
        public ResponseCoreData GetAll(int journal16Id, int skip, int take)
        {
            try
            {
                return _journal15Service.GetAll(UserId, journal16Id, skip, take);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/GetAll", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal15View)]
        public ResponseCoreData GetById(int Id)
        {
            try
            {
                return _journal15Service.GetById(UserId, Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/GetById", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal15Edit)]
        public ResponseCoreData Add([FromBody] Journal15ViewModel model)
        {
            try
            {
                return _journal15Service.Add(CompanyId, UserId, model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/Add", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.Journal15Edit)]
        public ResponseCoreData Update([FromBody] Journal15ViewModel model)
        {
            try
            {
                return _journal15Service.Update(CompanyId, UserId, model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/Update", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.Journal15Edit)]
        public ResponseCoreData DeleteById(int Id)
        {
            try
            {
                return _journal15Service.DeleteById(CompanyId, UserId, Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/DeleteById", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal15View)]
        public ResponseCoreData GetJournal16ByDate(DateTime date)
        {
            try
            {
                return _journal15Service.GetJournal16ByDate(date);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/GetJournal16ByDate", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal15View)]
        public ResponseCoreData IsDayClosed(DateTime date)
        {
            try
            {
                return _journal15Service.IsDayClosed(CompanyId, date);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/IsDayClosed", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseCoreData GetCurrencyList()
        {
            return _journal15Service.GetCurrencyList();
        }

        /// <summary>
        /// Export to Excel (international currencies)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal15View)]
        public async Task<FileContentResult> ExportToExcel([FromBody] Journal15ForEcxel model)
        {
            try
            {
                var file = _journal15Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
                var fileName = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}book15";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss")}book15Val.xlsx");
                System.IO.File.WriteAllBytes(path, file);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/ExportToExcel", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Export to Excel (Sum)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal15View)]
        public async Task<FileContentResult> ExportToExcelSum([FromBody] Journal15ForEcxel model)
        {
            try
            {
                var file = _journal15Service.ToExportSum(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
                var fileName = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}book15";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss")}book15Sum.xlsx");
                System.IO.File.WriteAllBytes(path, file);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal15Api/ExportToExcel", ex.Message);
                return null;
            }
        }
    }
}