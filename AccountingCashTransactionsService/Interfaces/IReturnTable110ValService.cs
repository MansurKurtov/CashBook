using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models;
using Entitys.Models.CashOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IReturnTable110ValService : IEntityRepositoryCore<Journal110Val>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseCoreData GetByDate(int bankCode, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="toCashierId"></param>
        /// <returns></returns>
        ResponseCoreData GetValCashValue(DateTime date, int toCashierId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        byte[] ToExport(Jourlan110ReturnExcelModel model, string user, int bankCode);

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
