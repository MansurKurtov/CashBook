using AccountingCashTransactionsService.Helper;
using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Enums;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.Collector;
using Entitys.ViewModels.CashOperation.Journal15;
using Entitys.ViewModels.CashOperation.Journal16VM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class CollectorService : EntityRepositoryCore<Collector>, ICollectorService
    {
        private CommonHelper _commonHelper;
        private DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexts"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public CollectorService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
            _commonHelper = new CommonHelper(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Collector ToEntity(CollectorViewModel model)
        {
            var result = new Collector();
            result.Journal16Id = model.Journal16Id;
            result.Fio = model.Fio;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private CollectorViewModel ToModel(Collector entity)
        {
            var result = new CollectorViewModel();
            result.Id = entity.Id;
            result.Journal16Id = entity.Id;
            result.Fio = entity.Fio;
            result.SystemDate = entity.SystemDate;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Add(int bankCode, int userId, CollectorViewModel model)
        {
            var entity = ToEntity(model);
            entity.SystemDate = DateTime.Now;
            Add(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Collector, EventType.Edit);

            return new ResponseCoreData(entity, ResponseStatusCode.OK);

        }

        public ResponseCoreData Add(int bankCode, int userId, List<CollectorViewModel> data)
        {
            _context.SaveChanges();
            data.ForEach(model =>
            {
                var entity = ToEntity(model);
                entity.SystemDate = DateTime.Now;
                _context.Entry<Collector>(entity).State = EntityState.Added;
                _context.Collectors.Add(entity);
                _context.SaveChanges();

                _context.Entry<Collector>(entity).State = EntityState.Detached;
            });

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Collector, EventType.Edit);

            return new ResponseCoreData(ResponseStatusCode.OK);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="journal16Id"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public ResponseCoreData GetAll(int userId, int journal16Id, int skip, int take)
        {
            var result = new CollectorResultViewModel();
            var findResult = Find(f => f.Journal16Id == journal16Id).Skip(skip * take).Take(take).ToList();
            result.Data = findResult.Select(ToModel).ToList();
            result.Total = findResult.Count;

            //_commonHelper.SaveUserEvent(userId, ModuleType.Collector, EventType.Read);

            return new ResponseCoreData(result, ResponseStatusCode.OK);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResponseCoreData GetById(int userId, int Id)
        {
            var entity = _context.Collectors.Where(f => f.Id == Id).ToList().FirstOrDefault();
            if (entity == null)
                return new ResponseCoreData(new Exception("Not Found"));
            var model = ToModel(entity);

            //_commonHelper.SaveUserEvent(userId, ModuleType.Collector, EventType.Read);

            return new ResponseCoreData(model, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Update(int bankCode, int userId, CollectorViewModel model)
        {

            var existsEntity = _context.Collectors.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
            if (existsEntity == null)
                return new ResponseCoreData(new Exception("Not Found"));

            var entity = ToEntity(model);
            entity.Id = model.Id ?? 0;
            Update(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Collector, EventType.Edit);

            return new ResponseCoreData(ResponseStatusCode.OK);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResponseCoreData DeleteById(int bankCode, int userId, int Id)
        {

            var entity = _context.Collectors.Where(f => f.Id == Id).ToList().FirstOrDefault();
            if (entity == null)
                return new ResponseCoreData(new Exception("Not Found"));

            Delete(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Collector, EventType.Delete);

            return new ResponseCoreData(ResponseStatusCode.OK);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetJournal16ByDate(DateTime date)
        {

            var entitys = _context.Journal16s.Where(w => w.Date == date).ToList();
            var result = entitys.Select(ConvertToBook16DropDownModel).ToList();

            return new ResponseCoreData(result, ResponseStatusCode.OK);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Journal16DropDownViewModel ConvertToBook16DropDownModel(Journal16 entity)
        {
            var result = new Journal16DropDownViewModel();
            result.Id = entity.Id;
            result.DirectionNumber = entity.DirectionNumber;

            return result;
        }
    }
}
