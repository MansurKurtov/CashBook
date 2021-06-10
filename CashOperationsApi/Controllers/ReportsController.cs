using System;
using System.IO;
using System.Threading.Tasks;
using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AuthService.Jwt;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.ViewModels.CashOperation;
using Microsoft.AspNetCore.Mvc;
using SB.Common;

namespace CashOperationsApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReportsController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        IReportsService _reportsService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportsService"></param>
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
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
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.ReportsView)]
        public async Task<ResponseCoreData> GetCashOperationsByCashier(DateTime fromDate, DateTime toDate)
        {
            return _reportsService.GetCashOperationsByCashiers(fromDate, toDate, BankKod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permission.ReportsView)]
        public async Task<ResponseCoreData> GetReport1Val(DateTime fromDate, DateTime toDate)
        {
            return _reportsService.GetReport1Val(fromDate, toDate, BankKod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileContentResult TestExcelSimpleReport()
        {
            return _reportsService.TestExcelSimpleReport();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseCoreData> GetReport1Cashiers(DateTime fromDate, DateTime toDate)
        {
            return _reportsService.CashOperByCashierRep1(fromDate, toDate, BankKod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseCoreData> GetBanks()
        {
            return _reportsService.GetBanks();
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public async Task<ResponseCoreData> Gettt(DateTime fromDate, DateTime toDate)
        {
            return _reportsService.GetReport2ValExemplar(fromDate, toDate, BankKod);
        }
        [HttpGet]
        public async Task<ResponseCoreData> GenerateReport2(DateTime fromDate, DateTime toDate)
        {
            return _reportsService.GetReport2Val(fromDate, toDate, BankKod);
        }
        [HttpPost]
        public async Task<FileContentResult> ExportToExcel([FromBody]Report2ExcelExport model)
        {            
            var file = _reportsService.ToExport(model);
            var fileName = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}book120";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}reports.xlsx");
            System.IO.File.WriteAllBytes(path, file);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }
    }
}