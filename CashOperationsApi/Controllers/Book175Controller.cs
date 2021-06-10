using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.ViewModels.CashOperation;
using Entitys.ViewModels.CashOperation.Book155;
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
    public class Book175Controller : ControllerBase
    {
        private readonly IBook175Service _book175Service;
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
        /// <param name="book175Service"></param>
        public Book175Controller(IBook175Service book175Service)
        {
            _book175Service = book175Service;
        }

        /// <summary>
        /// 
        /// </summary>
        private int FromCasheirId { get { return Convert.ToInt32(User.FindFirst("FromCasheirId")?.Value); } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book175View)]
        public ResponseCoreData GetAll()
        {
            return _book175Service.GetAll(UserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Book175View)]
        public ResponseCoreData GetByDate(DateTime date)
        {
            return _book175Service.GetByDate(UserId, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book175Edit)]
        public ResponseCoreData Add([FromBody] Book175PostViewModel model)
        {
            model.FromCashierId = FromCasheirId;
            model.From175 = true;
            return _book175Service.Add(CompanyId, UserId, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.Book175Edit)]
        public ResponseCoreData Update([FromBody] Book175PutViewModel model)
        {
            model.FromCashierId = FromCasheirId;
            return _book175Service.Update(CompanyId, UserId, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.Book175Edit)]
        public ResponseCoreData DeleteById(int Id)
        {
            return _book175Service.DelateById(CompanyId, UserId, Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Book175Edit)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<Book155ViewModels> model)
        {
            var file = _book175Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName), CompanyId);
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}book175";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}book175.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
    }
}