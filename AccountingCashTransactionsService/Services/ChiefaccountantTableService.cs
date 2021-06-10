using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.ChiefaccountantTable;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingCashTransactionsService.Services
{
    public class ChiefaccountantTableService : EntityRepositoryCore<ChiefaccountantTable>, IChiefaccountantTableService
    {
        DataContext _context;

        public ChiefaccountantTableService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResponseCoreData DeleteById(int Id)
        {
            try
            {
                var entity = _context.ChiefAccountantTables.Where(f => f.Id == Id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData(new Exception("Not Found"));

                Delete(entity);
                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResponseCoreData GetById(int Id)
        {
            try
            {
                var entity = _context.ChiefAccountantTables.Where(f => f.Id == Id).ToList().FirstOrDefault();

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData AddOrUpdate(ChiefaccountantTablePostViewModel model)
        {
            try
            {
                var entity = _context.ChiefAccountantTables.Where(f => f.BankKod == model.BankKod).ToList().FirstOrDefault();
                var NewModel =new ChiefaccountantTable();

                if (model.FIO == null)
                    return new ResponseCoreData(ResponseStatusCode.BadRequest);
                
                if (entity == null)
                {
                    NewModel.BankKod = model.BankKod;
                    NewModel.CreateDate = DateTime.Now;
                    NewModel.CreatedUserId = model.UserId;
                    NewModel.UpdateDate = DateTime.Now;
                    NewModel.UpdatedUserId = model.UserId;
                    NewModel.FIO = model.FIO;
                    _context.ChiefAccountantTables.Update(NewModel);
                }
                else
                {
                    if (entity.FIO == model.FIO)
                        return new Exception("такой пользователь существует");               

                    entity.UpdateDate = DateTime.Now;
                    entity.UpdatedUserId = model.UserId;
                    entity.FIO = model.FIO;
                    _context.ChiefAccountantTables.Update(entity);
                }                
                _context.SaveChanges();

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetAll(int bankKod)
        {
            try
            {
                var entity = Find(f => f.BankKod == bankKod).ToList().FirstOrDefault();

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
