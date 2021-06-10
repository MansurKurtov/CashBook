using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using Entitys.PostModels.CashOperations;
using Entitys.ViewModels.CashOperation.Journal15;
using System;
using System.Collections.Generic;
using Document = iTextSharp.text.Document;
namespace AccountingCashTransactionsService.Interfaces
{
    public interface IJournal15Service : IEntityRepositoryCore<Journal15>
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
        ResponseCoreData Add(int bankCode, int userId, Journal15ViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData Update(int bankCode, int userId, Journal15ViewModel model);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseCoreData IsDayClosed(int bankCode, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResponseCoreData GetCurrencyList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        byte[] ToExport(Journal15ForEcxel model, string user, int bankCode);
        byte[] ToExportSum(Journal15ForEcxel model, string user, int bankCode);
    }
}
