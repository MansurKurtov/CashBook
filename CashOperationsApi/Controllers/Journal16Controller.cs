using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.Models;
using Entitys.ViewModels.CashOperation.Journal16VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CashOperationsApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Journal16Controller : ControllerBase
    {

        private readonly IJournal16Service _journal16Service;
        private ILogger<Journal16Controller> _logger;

        private int BankKod
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("OrgId")?.Value);
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

        private int CashierTypeId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("CashierTypeId")?.Value);
            }
        }

        private int UserId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("UserId")?.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book16Service"></param>
        public Journal16Controller(IJournal16Service book16Service, ILogger<Journal16Controller> logger)
        {
            _journal16Service = book16Service;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Set16Aceeptance)]
        public ResponseCoreData SetAcceptance([FromBody] SetAcceptanceViewModel model)
        {
            if (model == null)
                return new ResponseCoreData(ResponseStatusCode.BadRequest);

            try
            {
                return _journal16Service.SetAcceptance(model.Id, model.Acceptance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal16Api/SetAcceptance", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="status"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal16View)]
        public ResponseCoreData GetAllWithJournal15(DateTime date, int? status, int skip, int take)
        {
            try
            {
                return _journal16Service.GetAllWithJournal15(BankKod, CashierTypeId, date, status, skip, take);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal16Api/GetAllWithJournal15", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal16View)]
        public ResponseCoreData GetById(int id)
        {
            try
            {
                return _journal16Service.GetById(UserId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal16Api/GetById", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal16Edit)]
        public ResponseCoreData Add([FromBody] Journal16PostViewModel model)
        {
            try
            {
                return _journal16Service.Add(BankKod, CashierTypeId, UserId, model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal16Api/Add", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.Journal16Edit)]
        public ResponseCoreData Update([FromBody] Journal16PutViewModel model)
        {
            try
            {
                return _journal16Service.Update(BankKod, UserId, model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal16Api/Update", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [CustomAuthorize(Permission.Journal16Edit)]
        [HttpDelete]
        public ResponseCoreData DeleteById(int Id)
        {
            try
            {
                return _journal16Service.DeleteById(BankKod, UserId, Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal16Api/DeleteById", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal16View)]
        public ResponseCoreData GetSupervisingAccountantFullName()
        {
            try
            {
                return _journal16Service.GetSupervisingAccountantFullName(BankKod);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal16Api/GetSupervisingAccountantFullName", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal16View)]
        public ResponseCoreData IsDayClosed(DateTime date)
        {
            try
            {
                return _journal16Service.IsDayClosed(BankKod, date);
            }
            catch (Exception ex)
            {
                _logger.LogError("Journal16Api/IsDayClosed", ex.Message);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal16Edit)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<ExcelModelForJournal16> model)
        {
            if (model.Count() > 0)
            {
                var file = _journal16Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), BankKod, CashierTypeId);
                var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}Journal18";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}Journal18.xlsx");
                System.IO.File.WriteAllBytes(path, file);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Set16Aceeptance)]
        public async Task<FileContentResult> SecondExportToExcel([FromBody] List<ExcelModelForJournal16Second> model)
        {
            var file = _journal16Service.SecondToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName));
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}Journal16";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}Journal16.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
    }
}