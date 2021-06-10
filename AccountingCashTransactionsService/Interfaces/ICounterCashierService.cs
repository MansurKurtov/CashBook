using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface ICounterCashierService : IEntityRepositoryCore<CounterCashier>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResponseCoreData GetAll(int bankCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseCoreData GetById(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData Add(CounterCashier model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData Update(CounterCashier model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseCoreData Delete(int Id);

    }
}
