using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.DB;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.PostModels.CashOperations;
using Entitys.ViewModels.CashOperation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class CommonService : ICommonService
    {
        /// <summary>
        /// 
        /// </summary>
        private DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public CommonService(DataContext context) 
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public ResponseCoreData GetChiefAccountantName(int bankId)
        {
            var result = new ChiefAccountantViewModel();
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = cmd.Connection.State == ConnectionState.Open;
                if (!wasOpen) cmd.Connection.Open();
                try
                {
                    cmd.CommandText = $"select GET_CHIEFACCOUNTANT_NAME({bankId}) from dual";
                    var scalaerResult = cmd.ExecuteScalar();
                    var accountantFullName = scalaerResult == DBNull.Value ? string.Empty : (string)scalaerResult;
                    result.FullName = accountantFullName;
                }
                finally
                {
                    if (!wasOpen) cmd.Connection.Close();
                }
            }

            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public ResponseCoreData GetBankName(int bankId)
        {
            var result = new BankNameViewModel();
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = cmd.Connection.State == ConnectionState.Open;
                if (!wasOpen) cmd.Connection.Open();
                try
                {
                    cmd.CommandText = $"select GET_BANK_NAME({bankId}) from dual";
                    var scalaerResult = cmd.ExecuteScalar();
                    var bankName = scalaerResult == DBNull.Value ? string.Empty : (string)scalaerResult;                    
                    result.BankName = bankName;
                }
                finally
                {
                    if (!wasOpen) cmd.Connection.Close();
                }
            }

            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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

        public  ResponseCoreData GetWorkingDate(int bankCode)
        {
            try
            {
                string sqltxt = $"select GET_WORKING_DATE({bankCode}) as result from dual";
                var entity = _context.Query<GetWorkingDates>().FromSql(sqltxt).ToList();
                var result = entity.FirstOrDefault().RESULT ?? DateTime.Today;

                return new ResponseCoreData(result.Date, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}