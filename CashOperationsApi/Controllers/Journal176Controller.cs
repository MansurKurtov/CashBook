using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.Models;
using Entitys.ViewModels.CashOperation.Journal176ViewModel;
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
    public class Journal176Controller : ControllerBase
    {
        private readonly IJournal176Service _journal176Service;
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

        private int BankKod
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("OrgId")?.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="journal176Service"></param>
        public Journal176Controller(IJournal176Service journal176Service)
        {
            _journal176Service = journal176Service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal176View)]
        public ResponseCoreData GetAll()
        {
            return _journal176Service.GetAll(UserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal176View)]
        public ResponseCoreData GetById(int Id)
        {
            return _journal176Service.GetById(UserId, Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal176View)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            return _journal176Service.GetByDate(UserId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal176Edit)]
        public ResponseCoreData Add([FromBody] Journal176PostViewModel model)
        {
            return _journal176Service.Add(BankKod, UserId, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.Journal176Edit)]
        public ResponseCoreData Update([FromBody] Journal176PutViewModel model)
        {
            return _journal176Service.Update(BankKod, UserId, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.Journal176Edit)]
        public ResponseCoreData Delete(int Id)
        {
            return _journal176Service.Delete(BankKod, UserId, Id);
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
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal176View)]
        public ResponseCoreData GetCounterCashiers()
        {
            return _journal176Service.GetCounterCashiers();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal176View)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<ExcelModelForJournal176> model)
        {
            byte[] file = _journal176Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}Journal176";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}Journal176.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal176View)]
        public ResponseCoreData Jumavoy(DateTime date, int status)
        {
            return _journal176Service.Jumavoy(date, status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal176View)]
        public ResponseCoreData IsDayClosed(DateTime date)
        {
            return _journal176Service.IsDayClosed(BankKod, date);
        }
    }
}