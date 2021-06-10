using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using Entitys.PostModels.CashOperations;
using Entitys.ViewModels.CashOperation.Book155;
using System;
using System.Collections.Generic;

namespace AccountingCashTransactionsService.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBook155Service : IEntityRepositoryCore<Book155>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="cashierTypeId"></param>
        /// <returns></returns>
        ResponseCoreData Add(int bankCode, Book155Model model, int userId, int cashierTypeId);

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
        /// <param name="orgId"></param>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <param name="operationId"></param>
        /// <param name="filterUserId"></param>
        /// <param name="sprObjectId"></param>
        /// <param name="worthAccount"></param>
        /// <returns></returns>
        ResponseCoreData GetAll(int orgId, DateTime date, int userId, int? operationId,
            int? filterUserId, int? sprObjectId, string worthAccount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseCoreData GetById(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="cashierTypeId"></param>
        /// <returns></returns>
        ResponseCoreData Update(int bankCode, Book155PutViewModel model, int userId, int cashierTypeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseCoreData SubmitAcceptance(AcceptanceViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        ResponseCoreData GetCashiers(int orgId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        ResponseCoreData GetReCounterCashiers(int orgId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        ResponseCoreData GetCashiersWithType(int orgId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResponseCoreData GetCurrencyTypes();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        ResponseCoreData GetWorthAccounts(int orgId);

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
        /// <param name="cashierTypeId"></param>
        /// <returns></returns>
        ResponseCoreData GetBook155OtherInouts(int cashierTypeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        byte[] ToExport(List<Book155ViewModels> model, string user, int bankCode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        DateTime GetWorkingDate(int orgId);

    }
}
