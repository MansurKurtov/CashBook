using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.Journal176ViewModel;
using System;
using System.Collections.Generic;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IJournal176Service : IEntityRepositoryCore<Journal176>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        byte[] ToExport(List<ExcelModelForJournal176> model, string user, int bankCode);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseCoreData GetAll(int userId);

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
        ResponseCoreData Add(int bankCode, int userId, Journal176PostViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData Update(int bankCode, int userId, Journal176PutViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseCoreData Delete(int bankCode, int userId, int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseCoreData GetByDate(int userId, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResponseCoreData GetCounterCashiers();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ResponseCoreData Jumavoy(DateTime date, int status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseCoreData IsDayClosed(int bankCode, DateTime date);
    }
}
