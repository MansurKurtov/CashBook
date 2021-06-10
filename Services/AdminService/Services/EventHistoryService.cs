using AdminService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Enums;
using Entitys.Models.Auth;
using Entitys.ViewModels.CashOperation.EventHistory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdminService.Services
{
    public class EventHistoryService : EntityRepositoryCore<EventHistory>, IEventHistoryService
    {
        /// <summary>
        /// 
        /// </summary>
        DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexts"></param>
        /// <param name="context"></param>
        public EventHistoryService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(DateTime fromDate, DateTime toDate, int skip, int take, int bankKod = 0)
        {
            try
            {
                var result = new ResultEventHistoryModel();
                if (bankKod == 0)
                {
                    var entity = _context.EventHistories.Where(f => f.SystemDate.Date >= fromDate.Date && f.SystemDate <= toDate.Date).ToList().Skip(skip * take).Take(take).Select(s => new EventHistoryViewModel()
                    {
                        Id = s.Id,
                        SystemDate = s.SystemDate,
                        UserName = GetUserName(_context.AuthUsers.Where(f => f.Id == s.UserId).ToList().FirstOrDefault()),
                        ModulName = s.ModuleId.ToString(),
                        UserEventType = s.UserEventType.ToString()
                    }).ToList();
                    result.Data = entity;
                    result.Total = _context.EventHistories.Where(f => (f.SystemDate.Date >= fromDate.Date && f.SystemDate <= toDate.Date) && f.BankKod == bankKod).Count();
                }
                else
                {
                    var entity = _context.EventHistories.Where(f => (f.SystemDate.Date >= fromDate.Date && f.SystemDate <= toDate.Date) && f.BankKod == bankKod).ToList().Skip(skip * take).Take(take).Select(s => new EventHistoryViewModel()
                    {
                        Id = s.Id,
                        SystemDate = s.SystemDate,
                        UserName = GetUserName(_context.AuthUsers.Where(f => f.Id == s.UserId).ToList().FirstOrDefault()),
                        ModulName = s.ModuleId.ToString(),
                        UserEventType = s.UserEventType.ToString()
                    }).ToList();
                    result.Data = entity;
                    result.Total = _context.EventHistories.Where(f => (f.SystemDate.Date >= fromDate.Date && f.SystemDate <= toDate.Date) && f.BankKod == bankKod).Count();
                }

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private string GetUserName(AuthUsers userModel)
        {
            string fullName = userModel.FirstName != null ? userModel.FirstName : string.Empty;
            fullName += " ";
            fullName += userModel.MiddleName != null ? userModel.MiddleName : string.Empty;
            fullName += " ";
            fullName += userModel.LastName != null ? userModel.LastName : string.Empty;

            return fullName;
        }
    }
}