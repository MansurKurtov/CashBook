using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.ViewModels.CashOperation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IReportsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        byte[] ToExport(Report2ExcelExport model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        ResponseCoreData GetCashOperationsByCashiers(DateTime fromDate, DateTime toDate, int bankCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        ResponseCoreData GetReport1Val(DateTime fromDate, DateTime toDate, int bankCode);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        FileContentResult TestExcelSimpleReport();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        ResponseCoreData CashOperByCashierRep1(DateTime fromDate, DateTime toDate, int bankCode);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResponseCoreData GetBanks();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        ResponseCoreData GetReport2Val(DateTime fromDate, DateTime toDate, int bankCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        ResponseCoreData GetReport2ValExemplar(DateTime fromDate, DateTime toDate, int bankCode);
    }
}
