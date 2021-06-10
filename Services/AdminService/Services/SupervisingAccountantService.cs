using AdminService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Models.CashOperation;
using Entitys.PostModels.CashOperations;
using System;
using System.Linq;

namespace AdminService.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SupervisingAccountantService : EntityRepositoryCore<SupervisingAccountant>, ISupervisingAccountantService
    {
        private readonly DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SupervisingAccountantService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
        }

        public SupervisingAccountant ToEntity(SupervisingAccountantPostModel model, int bankCode)
        {
            var result = new SupervisingAccountant();
            result.Fio = model.Fio;
            result.BankCode = bankCode;

            return result;
        }

        public static SupervisingAccountantViewModel ToViewModel(SupervisingAccountant entity)
        {
            var result = new SupervisingAccountantViewModel();
            result.Id = entity.Id;
            result.Fio = entity.Fio;
            result.BankCode = entity.BankCode;
            result.CreatedUserId = entity.CreatedUserId;
            result.CreateDate = entity.CreatedDate;
            result.UpdatedUserId = entity.UpdatedUserId;
            result.UpdatedDate = entity.UpdateDate;

            return result;
        }

        public ResponseCoreData GetSupAccountant(int bankCode)
        {
            try
            {
                var item = _context.SupervisingAccountants.Where(f => f.BankCode == bankCode).ToList().FirstOrDefault();
                if (item == null)
                    return new ResponseCoreData(item, ResponseStatusCode.OK);

                var result = ToViewModel(item);
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }


        public ResponseCoreData Add(SupervisingAccountantPostModel model, int bankCode, int userId)
        {
            try
            {
                var item = _context.SupervisingAccountants.Where(f => f.BankCode == bankCode).ToList().FirstOrDefault();
                if (item != null)
                    return Update(model, bankCode, userId);

                var entity = ToEntity(model, bankCode);
                entity.CreatedDate = DateTime.Now;
                entity.CreatedUserId = userId;
                Add(entity);
                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        public ResponseCoreData Update(SupervisingAccountantPostModel model, int bankCode, int userId)
        {
            try
            {
                var item = _context.SupervisingAccountants.Where(f => f.BankCode == bankCode).ToList().FirstOrDefault();
                if (item == null)
                    return new ResponseCoreData("Not found", ResponseStatusCode.BadRequest);
                item.BankCode = bankCode;
                item.Fio = model.Fio;
                item.UpdateDate = DateTime.Now;
                item.UpdatedUserId = userId;
                Update(item);
                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        public ResponseCoreData DeleteById(int Id)
        {
            try
            {
                var item = _context.SupervisingAccountants.Where(f => f.Id == Id).ToList().FirstOrDefault();
                if (item == null)
                    return new ResponseCoreData("Not found", ResponseStatusCode.BadRequest);
                Delete(item);
                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
    }
}
