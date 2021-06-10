using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [CustomAuthorize(Permission.CashBookAdmin)]
    public class EventHisoryController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        IEventHistoryService _eventHistoryService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHistoryService"></param>
        public EventHisoryController(IEventHistoryService eventHistoryService)
        {
            _eventHistoryService = eventHistoryService;
        }

        /// <summary>
        /// 
        /// </summary>
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
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseCoreData> GetByDate(DateTime fromDate, DateTime toDate, int skip, int take)
        {
            return _eventHistoryService.GetByDate(fromDate, toDate, skip, take, BankKod);
        }
    }
}