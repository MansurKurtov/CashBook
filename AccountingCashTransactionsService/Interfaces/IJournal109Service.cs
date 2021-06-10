using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models;
using Entitys.Models.CashOperation;
using System;
using System.Collections.Generic;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IJournal109Service : IEntityRepositoryCore<Journal109>
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
        /// <param name="user"></param>
        /// <returns></returns>
        byte[] ToExport(List<ExcelModel> model, string user, int bankCode);

    }
}
