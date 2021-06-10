using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.ChiefaccountantTable;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCashTransactionsService.Interfaces
{
    public interface IChiefaccountantTableService : IEntityRepositoryCore<ChiefaccountantTable>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ResponseCoreData GetAll(int bankKod);

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
        ResponseCoreData AddOrUpdate(ChiefaccountantTablePostViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseCoreData DeleteById(int Id);


    }
}
