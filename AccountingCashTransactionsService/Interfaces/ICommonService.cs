using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using System;
using System.Collections.Generic;

namespace AccountingCashTransactionsService.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommonService
    {
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
        /// <param name="bankCode"></param>
        /// <returns></returns>
        ResponseCoreData GetSupAccountant(int bankCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        ResponseCoreData GetWorkingDate(int bankCode);
    }
}
