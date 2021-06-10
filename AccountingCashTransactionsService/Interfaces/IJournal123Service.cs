using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.Journal123;
using System;
using System.Collections.Generic;

namespace AccountingCashTransactionsService.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJournal123Service : IEntityRepositoryCore<Journal123>
    {
        ResponseCoreData AddDate(int bankCode, int userId, Journal123PostModel model);
        ResponseCoreData UpdateDate(int bankCode, int userId, Journal123UpdateModel model);
        ResponseCoreData GetDateById(int id);
        ResponseCoreData GetDateByDate(DateTime date);
        ResponseCoreData DeleteDateById(int bankCode, int userId, int id);
        
        ResponseCoreData AddContent(int bankCode, int userId, Journal123ContentPostModel model);
        ResponseCoreData UpdateContent(int bankCode, int userId, Journal123ContentUpdateModel model);
        ResponseCoreData GetContentById(int id);
        ResponseCoreData GetContentByDate(DateTime date);
        ResponseCoreData GetContentsByJournal123Id(int journal123Id);
        ResponseCoreData DeleteContentById(int bankCode, int userId, int id);

        ResponseCoreData AddFio(int bankCode, int userId, Journal123FioPostModel model);
        ResponseCoreData UpdateFio(int bankCode, int userId, Journal123FioUpdateModel model);
        ResponseCoreData GetFioById(int id);
        ResponseCoreData GetFioByDate(DateTime date);
        ResponseCoreData GetFiosByJournal123Id(int journal123Id);
        ResponseCoreData DeleteFioById(int bankCode, int userId, int id);
        byte[] ToExport(List<ExcelFor123Froms> model, string user);

    }
}
