using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models;
using Entitys.Models.CashOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IJournal109WorthService : IEntityRepositoryCore<Journal109Worth>
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
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        byte[] ToExport(List<ExcelModel> model, string user, int bankCode);
    }
}
