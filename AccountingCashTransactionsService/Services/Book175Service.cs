using AvastInfrastructureRepository.Repositories.Services;
using EntityRepository.Context;
using Entitys.ViewModels.CashOperation;
using System;
using AutoMapper;
using Entitys.Models.CashOperation;
using System.Linq;
using AccountingCashTransactionsService.Interfaces;
using Entitys.DB;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using Entitys.ViewModels.CashOperation.Book155;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using Entitys.Helper.ExcelAround;
using AccountingCashTransactionsService.Helper;
using Entitys.Enums;
using Microsoft.EntityFrameworkCore;
using Entitys.Models;
using System.Data;

namespace AccountingCashTransactionsService.Services
{
    public class Book175Service : EntityRepositoryCore<Book175>, IBook175Service
    {
        private DataContext _context;
        private IMapper _mapper;
        private CommonHelper _commonHelper;

        public Book175Service(IDbContext context, IMapper mapper, DataContext contexts) : base(context)
        {
            _mapper = mapper;
            _context = contexts;
            _commonHelper = new CommonHelper(contexts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Add(int bankCode, int userId, Book175PostViewModel model)
        {
            try
            {
                var entity = _mapper.Map<Book175PostViewModel, Book155>(model);
                entity.From175 = true;
                _context.Book155s.Add(entity);

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book175, EventType.Edit);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResponseCoreData DelateById(int bankCode, int userId, int Id)
        {
            try
            {
                var entity = _context.Book155s.Where(f => f.Id == Id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData(new Exception("Not Found"));

                _context.Book155s.Remove(entity);

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book175, EventType.Delete);

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseCoreData GetAll(int userId)
        {
            try
            {
                var entity = _context.Book155s.Where(f => f.Id != 0 && f.From175 == false).Select(s => new Book175ViewModel
                {
                    Accept = s.Accept,
                    CashValue = s.CashValue,
                    Date = s.Date,
                    FromCasheirId = s.FromCashierId,
                    OperationId = s.OperationId,
                    ToCashierId = s.ToCashierId
                }).ToList();

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(int userId, DateTime date)
        {
            try
            {
                var entity = _context.Book155s.Where(f => f.Date == date && f.From175 == false).ToList().Select(s => new Book175ViewModel
                {
                    Accept = s.Accept,
                    CashValue = s.CashValue,
                    Date = s.Date,
                    FromCasheirId = s.FromCashierId,
                    OperationId = s.OperationId,
                    ToCashierId = s.ToCashierId
                }).FirstOrDefault();

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Update(int bankCode, int userId, Book175PutViewModel model)
        {
            try
            {
                var entitys = _context.Book155s.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
                if (entitys == null)
                    return new ResponseCoreData(new Exception("Not Found"));

                var entity = _mapper.Map(model, entitys);
                entity.From175 = true;
                _context.Book155s.Update(entity);

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book175, EventType.Edit);

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        private DateTime GetWorkingDate(int bankCode)
        {
            try
            {
                string sqltxt = $"select GET_WORKING_DATE({bankCode}) as result from dual";
                var entity = _context.Query<GetWorkingDates>().FromSql(sqltxt).ToList().FirstOrDefault();
                var result = entity.RESULT ?? DateTime.Today;

                return result.Date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetBankNameById(int bankId)
        {
            var result = "---";
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = cmd.Connection.State == ConnectionState.Open;
                if (!wasOpen) cmd.Connection.Open();
                try
                {
                    cmd.CommandText = $"select GET_BANK_NAME({bankId}) from dual";
                    var scalaerResult = cmd.ExecuteScalar();
                    var bankName = scalaerResult == DBNull.Value ? string.Empty : (string)scalaerResult;
                    result = bankName;
                }
                finally
                {
                    if (!wasOpen) cmd.Connection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public byte[] ToExport(List<Book155ViewModels> model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();

                var ws = excel.Workbook.Worksheets.Add("Book175");
                int a1 = 7;
                int[] a = { 5, 5 };
                int[] b = { 6, 5 };
                string[] s = { "B", "C" };
                string[] s1 = { "B", "C", "D" };
                string[] d = { "B", "D" };
                string[] c = { "ФИШ", "Сумма" };
                for (int i = 0; i < s.Length; i++)
                {
                    ws.Column(i + 2).Width = 30;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Value = $"{c[i]}";
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Merge = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a[i]}", $"{d[i]}{b[i]}");
                }
                ws.Column(4).Width = 20;
                ws.Column(3).Width = 20;
                ws.Cells["C6"].Value = "Кирим";
                ws.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["D6"].Value = "Чиқим";
                ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["D6"].Style.Font.Bold = true;
                ws.Cells["C6"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"C6");
                ExcelAroundBorder.AroundBorder(ws, $"D6");
                foreach (var item in model)
                {
                    for (int i = 0; i < s1.Length; i++)
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s1[i]}{a1}");
                        ws.Cells[$"{s1[i]}{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    ws.Cells["C2"].Value = "Сана";
                    ws.Cells[$"D2"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells["C3"].Value = "Тип операции";
                    ws.Cells["D3"].Value = $"{item.FromCashierName}";
                    ws.Cells[$"B{a1}"].Value = $"{item.ToCashierName}";
                    ws.Cells[$"C{a1}"].Value = $"{0}";
                    ws.Cells[$"D{a1}"].Value = $"{item.CashValue}";
                    a1++;
                }
                ws.Cells[$"C{a1 + 2}"].Value = $"Ижрочи:";
                ws.Cells[$"D{a1 + 2}"].Value = $"{user}";
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
