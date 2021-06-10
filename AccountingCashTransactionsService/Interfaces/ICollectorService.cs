using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.Collector;
using System;
using System.Collections.Generic;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface ICollectorService : IEntityRepositoryCore<Collector>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="journal16Id"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        ResponseCoreData GetAll(int userId, int journal16Id, int skip, int take);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseCoreData GetById(int userId, int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData Add(int bankCode, int userId, CollectorViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        ResponseCoreData Add(int bankCode, int userId, List<CollectorViewModel> data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData Update(int bankCode, int userId, CollectorViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseCoreData DeleteById(int bankCode, int userId, int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResponseCoreData GetJournal16ByDate(DateTime date);
    }
}
