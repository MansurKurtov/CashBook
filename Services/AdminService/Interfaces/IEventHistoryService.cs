using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminService.Interfaces
{
    public interface IEventHistoryService : IEntityRepositoryCore<EventHistory>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        ResponseCoreData GetByDate(DateTime fromDate, DateTime toDate, int skip, int take, int bankKod);
    }
}
