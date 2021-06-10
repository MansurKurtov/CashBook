using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Helper.UserName;
using Entitys.Models;
using Entitys.ViewModels.CashOperation.Journal123;
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
    public class Journal123Controller : ControllerBase
    {
        private readonly IJournal123Service _journal123Service;
        private int UserId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("UserId")?.Value);
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
        /// <param name="journal123Service"></param>
        public Journal123Controller(IJournal123Service journal123Service)
        {
            _journal123Service = journal123Service;
        }

        /// <summary>
        /// Addes Date to Journal123 table
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData AddDate([FromBody]Journal123PostModel model)
        {
            return _journal123Service.AddDate(CompanyId, UserId, model);
        }

        /// <summary>
        /// Updates Date in Journal123 table by its model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData UpdateDate([FromBody]Journal123UpdateModel model)
        {
            return _journal123Service.UpdateDate(CompanyId, UserId, model);
        }

        /// <summary>
        /// Returns Date from Journal123 table by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal123View)]
        public ResponseCoreData GetDateById(int id)
        {
            return _journal123Service.GetDateById(id);
        }

        /// <summary>
        /// Returns row from Journal123 table by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal123View)]
        public ResponseCoreData GetDateByDate(DateTime date)
        {
            return _journal123Service.GetDateByDate(date);
        }
        /// <summary>
        /// Deletes Date row in Journal123 by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData DeleteDateById(int id)
        {
            return _journal123Service.DeleteDateById(CompanyId, UserId, id);
        }

        /// <summary>
        /// Addes Content in Journal123_Content by it's model data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData AddContent([FromBody]Journal123ContentPostModel model)
        {
            return _journal123Service.AddContent(CompanyId, UserId, model);
        }

        /// <summary>
        /// Updates Content in Journal123_Content table by model data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData UpdateContent([FromBody] Journal123ContentUpdateModel model)
        {
            return _journal123Service.UpdateContent(CompanyId, UserId, model);
        }

        /// <summary>
        /// Returns Content model by it's Id from Journal123_Content table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal123View)]
        public ResponseCoreData GetContentById(int id)
        {
            return _journal123Service.GetContentById(id);
        }

        /// <summary>
        /// Return Content by given date param
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal123View)]
        public ResponseCoreData GetContentByDate(DateTime date)
        {
            return _journal123Service.GetContentByDate(date);
        }

        /// <summary>
        /// Returns Content by it's journal123_Id key from Journal123_Content table
        /// </summary>
        /// <param name="journal123Id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal123View)]
        public ResponseCoreData GetContentsByJournal123Id(int journal123Id)
        {
            return _journal123Service.GetContentsByJournal123Id(journal123Id);
        }

        /// <summary>
        /// Deletes Content in Journal123_Content table by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData DeleteContentById(int id)
        {
            return _journal123Service.DeleteContentById(CompanyId, UserId, id);
        }

        /// <summary>
        /// Addes Fio to Journal123_FIO table
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData AddFio([FromBody]Journal123FioPostModel model)
        {
            return _journal123Service.AddFio(CompanyId, UserId, model);
        }

        /// <summary>
        /// Updates Journal123_FIO data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData UpdateFio([FromBody]Journal123FioUpdateModel model)
        {
            return _journal123Service.UpdateFio(CompanyId, UserId, model);
        }

        /// <summary>
        /// Returns FIO by it's id from Journal123_fio data table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal123View)]
        public ResponseCoreData GetFioById(int id)
        {
            return _journal123Service.GetFioById(id);
        }

        /// <summary>
        /// Returns FIO collection by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal123View)]
        public ResponseCoreData GetFioByDate(DateTime date)
        {
            return _journal123Service.GetFioByDate(date);
        }

        /// <summary>
        /// Returns Fio by Journal123_id
        /// </summary>
        /// <param name="journal123Id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.Journal123View)]
        public ResponseCoreData GetFiosByJournal123Id(int journal123Id)
        {
            return _journal123Service.GetFiosByJournal123Id(journal123Id);
        }

        /// <summary>
        /// Deletes Journal123_FIO data by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustomAuthorize(Permission.Journal123Edit)]
        public ResponseCoreData DeleteFioById(int id)
        {
            return _journal123Service.DeleteFioById(CompanyId, UserId, id);
        }

        /// <summary>
        /// Export to Excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[CustomAuthorize(Permission.Book120View)]
        public async Task<FileContentResult> ExportToExcel([FromBody] List<ExcelFor123Froms> model)
        {
            var file = _journal123Service.ToExport(model, PutUserName.GetPutUser(FirstName, MiddleName, LastName));
            var fileName = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}journal123";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}journal123.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
    }
}