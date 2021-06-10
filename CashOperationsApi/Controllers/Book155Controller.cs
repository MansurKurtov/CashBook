using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.PostModels.CashOperations;
using Entitys.ViewModels.CashOperation.Book155;
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
    [CustomAuthorize("")]
    [Route("api/[controller]/[action]")]
    public class Book155Controller : ControllerBase
    {
        private readonly IBook155Service _book155Service;
        private ILogger<Book155Controller> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book155Service"></param>
        /// <param name="logger"></param>

        public Book155Controller(
            IBook155Service book155Service,
            ILogger<Book155Controller> logger
            )
        {
            _book155Service = book155Service;
            _logger = logger;
        }

        private int CompanyId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("OrgId")?.Value);
            }
        }
        private int UserId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("UserId")?.Value);
            }
        }

        private int CashierTypeId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("CashierTypeId")?.Value);
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
        /// <param name="model"></param>
        /// <returns>
        /// asdasdasdasdasd
        /// </returns>
       // [Authorize]
        [HttpPost]
        [CustomAuthorize(Permission.Book155Edit)]
        public ResponseCoreData Add([FromBody]Book155Model model)
        {
            try
            {
                return _book155Service.Add(CompanyId, model, UserId, CashierTypeId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/Add", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpDelete]
        [CustomAuthorize(Permission.Book155Edit)]
        public ResponseCoreData Delete(int id)
        {
            try
            {
                return _book155Service.DeleteById(CompanyId, UserId, id);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/Delete", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="operationId"></param>
        /// <param name="filterUserId"></param>
        /// <param name="sprObjectId"></param>
        /// <param name="worthAccount"></param>
        /// <returns>
        /// asdasdasd
        /// </returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book155View)]
        public ResponseCoreData GetAll(DateTime date, int? operationId,
            int? filterUserId, int? sprObjectId, string worthAccount)
        {
            try
            {
                return _book155Service.GetAll(CompanyId, date, UserId, operationId, filterUserId, sprObjectId, worthAccount);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/GetAll", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [CustomAuthorize(Permission.Book155View)]
        public ResponseCoreData GetById(int id)
        {
            try
            {
                return _book155Service.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book155Api/GetById", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPut]
        [CustomAuthorize(Permission.Book155Edit)]
        public ResponseCoreData Update([FromBody]Book155PutViewModel model)
        {
            if (UserId == 0)
                return ResponseStatusCode.Unauthorized;

            try
            {
                return _book155Service.Update(CompanyId, model, UserId, CashierTypeId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/Update", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book155Edit)]
        public ResponseCoreData SubmitAcceptance([FromBody]AcceptanceViewModel model)
        {
            try
            {
                return _book155Service.SubmitAcceptance(model);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/SubmitAcceptance", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book155View)]
        public ResponseCoreData GetCashiers()
        {
            try
            {
                return _book155Service.GetCashiers(CompanyId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/GetCashiers", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book155View)]
        public ResponseCoreData GetReCounterCashiers()
        {
            try
            {
                return _book155Service.GetReCounterCashiers(CompanyId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/GetReCounterCashiers", ex.Message);
                return new ResponseCoreData(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book155View)]
        public ResponseCoreData GetCashiersWithType()
        {
            try
            {
                return _book155Service.GetCashiersWithType(CompanyId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/GetCashiersWithType", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book155Edit)]
        public ResponseCoreData GetCurrencyTypes()
        {
            try
            {
                return _book155Service.GetCurrencyTypes();
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/GetCurrencyTypes", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book155View)]
        public ResponseCoreData GetWorthAccounts()
        {
            try
            {
                return _book155Service.GetWorthAccounts(CompanyId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Book155Api/GetWorthAccounts", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book155View)]
        public ResponseCoreData IsDayClosed(DateTime date)
        {
            try
            {
                return _book155Service.IsDayClosed(CompanyId, date);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/IsDayClosed", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book155Edit)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<Book155ViewModels> model)
        {
            try
            {
                foreach (var item in model)
                    item.UserId = UserId;
                var file = _book155Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
                var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book155";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book155.xlsx");
                System.IO.File.WriteAllBytes(path, file);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/ExportToExcel", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book155View)]
        public ResponseCoreData GetBook155OtherInouts()
        {
            try
            {
                return _book155Service.GetBook155OtherInouts(CashierTypeId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Book155Api/GetBook155OtherInouts", ex.Message);
                return new ResponseCoreData(ex);
            }
        }
    }
}