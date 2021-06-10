using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.Journal16VM;
using System;
using System.Collections.Generic;

namespace AccountingCashTransactionsService.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJournal16Service : IEntityRepositoryCore<Journal16>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="acceptance"></param>
        /// <returns></returns>
        ResponseCoreData SetAcceptance(int id, bool acceptance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cashierTypeId"></param>
        /// <param name="date"></param>
        /// <param name="status"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        ResponseCoreData GetAllWithJournal15(int orgId, int cashierTypeId, DateTime date, int? status, int skip, int take);

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
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseCoreData GetByDate(int userId, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="cashierTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData Add(int bankCode, int cashierTypeId, int userId, Journal16PostViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData Update(int bankCode, int userId, Journal16PutViewModel model);

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
        /// <param name="bankCode"></param>
        /// <returns></returns>
        ResponseCoreData GetSupervisingAccountantFullName(int bankCode);

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
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        byte[] ToExport(List<ExcelModelForJournal16> model, string user, int bankCode, int CashierTypeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        byte[] SecondToExport(List<ExcelModelForJournal16Second> model, string user);
    }
}
