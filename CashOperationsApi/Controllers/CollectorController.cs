using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.ViewModels.CashOperation.Collector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CashOperationsApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CollectorController : ControllerBase
    {
        private readonly ICollectorService _collectorService;
        private ILogger<CollectorController> _logger;
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
        /// <param name="collectorService"></param>
        public CollectorController(ICollectorService collectorService, ILogger<CollectorController> logger )
        {
            _logger = logger;
            _collectorService = collectorService;
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
            return _collectorService.GetAll(UserId, journal16Id, skip, take);
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
                return _collectorService.GetById(UserId, Id);
                //_logger.LogInformation("",)
            }
            catch (Exception ex)
            {
                _logger.LogError("Collectorapi/GetById", ex.Message);
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
        public ResponseCoreData Add([FromBody]CollectorViewModel model)
        {
            try
            {
                return _collectorService.Add(CompanyId, UserId, model);
            }
            catch (Exception ex)
            {
                var data = JsonConvert.SerializeObject(model);
                _logger.LogError("CollectorApi/Add", ex.Message,":","Model:",data);
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permission.Journal15Edit)]
        public ResponseCoreData AddCollection([FromBody]List<CollectorViewModel> data)
        {
            try
            {
                return _collectorService.Add(CompanyId, UserId, data);
            }
            catch (Exception ex)
            {
                var model = JsonConvert.SerializeObject(data);
                _logger.LogError("CollectorApi/AddCollection", ex.Message, ":", "Model:", model);
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
        public ResponseCoreData Update([FromBody] CollectorViewModel model)
        {
            try
            {
                return _collectorService.Update(CompanyId, UserId, model);
            }
            catch(Exception ex)
            {
                var data = JsonConvert.SerializeObject(model);
                _logger.LogError("CollectorApi/Update", ex.Message, ":", "Model:", data);
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
                return _collectorService.DeleteById(CompanyId, UserId, Id);
            }
            catch(Exception ex)
            {
                _logger.LogError("CollectorApi/DeleteById", ex.Message, ":", "Id:", Id);
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
            return _collectorService.GetJournal16ByDate(date);
        }
    }
}