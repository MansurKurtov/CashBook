using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation;
using System;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IJournal111Service : IEntityRepositoryCore<Journal111>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseCoreData GetByDate(int bankCode, int userId, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] ToExport(int bankCode, Journal111ExcelPostModel model, string user);
    }
}
