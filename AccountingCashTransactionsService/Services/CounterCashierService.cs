using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.Context;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Models.CashOperation;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingCashTransactionsService.Services
{
    public class CounterCashierService : EntityRepositoryCore<CounterCashier>, ICounterCashierService
    {
        DataContext _context;

        public CounterCashierService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
        }

        public ResponseCoreData GetAll(int bankCode)
        {
            try
            {
                var entity = Find(f => f.Active == true && f.BankCode == bankCode).ToList();

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ResponseCoreData GetById(int Id)
        {
            try
            {
                var entity = _context.CounterCashiers.Where(f => f.Id == Id).ToList().FirstOrDefault();

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ResponseCoreData Add(CounterCashier model)
        {
            try
            {
                if (model == null || model.Name == null || model.BankCode == 0)
                    return new ResponseCoreData(ResponseStatusCode.BadRequest);

                _context.CounterCashiers.Add(model);
                _context.SaveChanges();

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ResponseCoreData Delete(int Id)
        {
            try
            {
                var entity = _context.CounterCashiers.Where(f => f.Id == Id).ToList().FirstOrDefault();

                if (entity == null)
                    return new ResponseCoreData(ResponseStatusCode.NotFound);

                Delete(entity);

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ResponseCoreData Update(CounterCashier model)
        {
            try
            {
                var entity = _context.CounterCashiers.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
                if (model == null || model.Name == null || model.BankCode == 0)
                    return new ResponseCoreData(ResponseStatusCode.BadRequest);

                if (entity == null)
                    return new ResponseCoreData(ResponseStatusCode.NotFound);

                entity.Name = model.Name;
                entity.BankCode = model.BankCode;
                entity.Active = model.Active;

                _context.CounterCashiers.Update(entity);
                _context.SaveChanges();

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
