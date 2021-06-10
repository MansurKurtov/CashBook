using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models;
using Entitys.Models.CashOperation;
using System;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IBook110Service : IEntityRepositoryCore<Journal110>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseCoreData GetAll(int bankCode, int userId, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        byte[] ToExport(int bankCode, Journal110ExcelModel model, string user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <param name="operationId"></param>
        /// <returns></returns>
        ResponseCoreData GetAll155CashValue(DateTime date, int userId, int operationId);
    }
}
