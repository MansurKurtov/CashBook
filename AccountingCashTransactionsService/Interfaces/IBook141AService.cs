using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models;
using Entitys.Models.CashOperation;
using System;
using System.Collections.Generic;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IBook141AService : IEntityRepositoryCore<Book141A>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseCoreData GeByDate(int bankCode, int userId, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="permissions"></param>
        /// <param name="date"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        ResponseCoreData SetFirstSaldo(int bankCode, string permissions, DateTime date, int saldoBeginCount, double saldoBeginSumma);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        ResponseCoreData GetChiefAccountantName(int bankId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        ResponseCoreData GetBankName(int bankId);



        /// <summary> 
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        byte[] ToExport(List<ExcelModelForBook141> model, string user, int bankCode);
    }
}
