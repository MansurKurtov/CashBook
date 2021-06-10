using AccountingCashTransactionsService.Helper;
using AccountingCashTransactionsService.Interfaces;
using AutoMapper;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Enums;
using Entitys.Helper.ExcelAround;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation;
using Entitys.ViewModels.CashOperation.Book155;
using Entitys.ViewModels.CashOperation.Journal15;
using Entitys.ViewModels.CashOperation.Journal176ViewModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class Journal176Service : EntityRepositoryCore<Journal176>, IJournal176Service
    {
        private IMapper _mapper;
        private DataContext _context;
        private CommonHelper _commonHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        /// <param name="contexts"></param>
        public Journal176Service(IMapper mapper, DataContext context, IDbContext contexts) : base(contexts)
        {
            _mapper = mapper;
            _context = context;
            _commonHelper = new CommonHelper(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Add(int bankCode, int userId, Journal176PostViewModel model)
        {
            try
            {
                var entity = _mapper.Map<Journal176PostViewModel, Journal176>(model);
                entity.RealTime = DateTime.Now;
                entity.Date = GetWorkingDate(bankCode);
                Add(entity);

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book176, EventType.Edit);

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
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseCoreData GetAll(int userId)
        {
            try
            {
                var entity = _context.Journal176s.Include(f => f.CounterCashierModel)
                     .Select(s => new Journal176ViewModels
                     {
                         Id = s.Id,
                         Date = s.Date,
                         BagNumber = s.BagNumber,
                         Summa = s.Summa,
                         LackSumma = s.LackSumma,
                         WorthlessSumma = s.WorthlessSumma,
                         ReceiptCount = s.ReceiptCount,
                         ReceiptSumma = s.ReceiptSumma,
                         Comment = s.Comment,
                         UserId = s.UserId,
                         CounterCashierId = s.CounterCashierId,
                         CounterCashierName = s.CounterCashierModel != null ? s.CounterCashierModel.Name : string.Empty,
                         ExcessSumma = s.ExcessSumma,
                         RealTime = s.RealTime,
                         Status = s.Status,
                         FakeSumma = s.FakeSumma

                     }).ToList();
                var result = new Journal176Result();
                result.Data = entity;
                result.Total = entity.Count();

                //_commonHelper.SaveUserEvent(userId, ModuleType.Book176, EventType.Read);

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
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResponseCoreData GetById(int userId, int Id)
        {
            try
            {
                var entity = _context.Journal176s.Where(f => f.Id == Id)
                    .Include(f => f.CounterCashierModel).ToList()
                    .Select(s => new Journal176ViewModels
                    {
                        Id = s.Id,
                        Date = s.Date,
                        BagNumber = s.BagNumber,
                        Summa = s.Summa,
                        LackSumma = s.LackSumma,
                        WorthlessSumma = s.WorthlessSumma,
                        ReceiptCount = s.ReceiptCount,
                        ReceiptSumma = s.ReceiptSumma,
                        Comment = s.Comment,
                        UserId = s.UserId,
                        CounterCashierId = s.CounterCashierId,
                        CounterCashierName = s.CounterCashierModel != null ? s.CounterCashierModel.Name : string.Empty,
                        ExcessSumma = s.ExcessSumma,
                        RealTime = s.RealTime,
                        Status = s.Status,
                        FakeSumma = s.FakeSumma

                    }).FirstOrDefault();

                //_commonHelper.SaveUserEvent(userId, ModuleType.Book176, EventType.Read);

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
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(int userId, DateTime date)
        {
            
            try
            {
                var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
                currencyTypes.Add(new GetValType { Kod = 0, Name = "Сўм" });
                var entity = _context.Journal176s.Where(f => f.Date == date)
                    .Include(f => f.CounterCashierModel)
                    .Select(s => new Journal176ViewModels
                    {
                        Id = s.Id,
                        Date = s.Date,
                        BagNumber = s.BagNumber,
                        Summa = s.Summa,
                        LackSumma = s.LackSumma,
                        WorthlessSumma = s.WorthlessSumma,
                        ReceiptCount = s.ReceiptCount,
                        ReceiptSumma = s.ReceiptSumma,
                        Comment = s.Comment,
                        UserId = s.UserId,
                        CounterCashierId = s.CounterCashierId,
                        CounterCashierName = s.CounterCashierModel != null ? s.CounterCashierModel.Name : string.Empty,
                        ExcessSumma = s.ExcessSumma,
                        RealTime = s.RealTime,
                        Status = s.Status,
                        FakeSumma = s.FakeSumma,
                        Date16 = s.Date16,
                        SprObjectId = s.SprObjectId,
                        CurrencyName =  currencyTypes.Where(w => w.Kod == s.SprObjectId).FirstOrDefault().Name
                    }).ToList();
                var result = new Journal176Result();
                result.Data = entity;
                result.Total = entity.Count();

                //_commonHelper.SaveUserEvent(userId, ModuleType.Book176, EventType.Read);

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ResponseCoreData Jumavoy(DateTime date, int status)
        {
            try
            {               
                var journal16Item = _context.Journal16s.Where(f => f.Date == date && f.Status == status).ToList();
                List<JumavoyViewModel> result = null;
                var res = _context.Journal176s.Where(w => w.Date16 == date && w.Status == status);
                if (_context.Journal176s.Where(w => w.Date16 == date && w.Status == status).Count() > 0)
                {
                    result = _context.Journal176s.Where(w => w.Date16 == date && w.Status == status).GroupBy(g => g.SprObjectId)
                       .Select(s => new JumavoyViewModel()
                       {
                           SprObjectId = s.Key,
                           TotalSumma = s.Sum(x => x.Summa),
                           TotalLackSumma = s.Sum(x => x.LackSumma),
                           TotalWorthlessSumma = s.Sum(x => x.WorthlessSumma),
                           TotalExcessSumma = s.Sum(x => x.ExcessSumma),
                           TotalFakeSumma = s.Sum(x => x.FakeSumma),
                           CurrencyName = "",
                           AcceptDate = journal16Item.FirstOrDefault().AcceptDate
                       }).ToList();
                    result.ForEach(f => f.CurrencyName = GetCurrencyTypeById(f.SprObjectId));
                }

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        private string GetCurrencyTypeById(int id)
        {
            var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
            var result = currencyTypes.Where(w => w.Kod == id).FirstOrDefault();
            if (result?.Name == null)
                return "Сўм";

            return result?.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Update(int bankCode, int userId, Journal176PutViewModel model)
        {
            try
            {
                var entity = _context.Journal176s.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData(ResponseStatusCode.NotFound);

                var models = _mapper.Map(model, entity);
                models.Date = GetWorkingDate(bankCode);
                _context.Journal176s.Update(models);
                _context.SaveChanges();

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book176, EventType.Edit);

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
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResponseCoreData Delete(int bankCode, int userId, int Id)
        {
            try
            {
                var entity = _context.Journal176s.Where(f => f.Id == Id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData(ResponseStatusCode.NotFound);

                _context.Journal176s.Remove(entity);
                _context.SaveChanges();

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book176, EventType.Delete);

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
        public ResponseCoreData GetCounterCashiers()
        {
            try
            {
                var users = _context.CounterCashiers.ToList();
                var result = users.Select(ConvertToCashierViewModel);
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
        /// <param name="cashier"></param>
        /// <returns></returns>
        private CashierViewModel ConvertToCashierViewModel(CounterCashier cashier)
        {
            var result = new CashierViewModel();
            result.Id = cashier.Id;
            result.FullName = $"{cashier.Name}";

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData IsDayClosed(int bankCode, DateTime date)
        {
            try
            {
                var commonHelper = new CommonHelper(_context);
                var result = new DayClosedViewModel();
                result.IsDayClosed = commonHelper.IsDayClosed((int)ModuleType.Book176, bankCode, date);

                return new ResponseCoreData(result, ResponseStatusCode.OK);
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
        public byte[] ToExport(List<ExcelModelForJournal176> model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var excel = new ExcelPackage();

                var ws = excel.Workbook.Worksheets.Add("Journal176");
                var a1 = 13;
                string[] s =    { "B",  "C",    "D",    "E",    "F",    "G",    "H",    "I",    "J",    "K" };
                int[] a =       { 9,    10,     11,     12,     12,     12,     11,     11,     11,      9};
                int[] b =       { 12,   12,     12,     12,     12,     12,     12,     12,     12,     12};
                string[] c = { " Боғлам(қоп) рақами ", "Валюта номи", " Умумий суммаси ", " Ортиқча пуллар ", " Камомад суммаси ", " Тўловга яроқсиз / Қалбаки пуллар ", " Чеклар умумий сони ", "Чеклар умумий суммаси ", " Изоҳлар ", " Сановчи кассир ФИО " };

                string[] d1 = { "C", "D", "H", "E" };
                string[] d2 = { "J", "G", "J", "G" };
                int[] DWidth = { 9, 10, 10, 11 };
                string[] Dwords = { "Боғлам(қоп) га солинган накд пул тушумлари бўйича ", " Накд пул тушумлари бўйича ", " Ҳисоб-китоб чеклари бўйича ", " Шу жумладан " };

                int[] width = { 25, 30, 20, 20, 20, 35, 25, 25, 20, 25 };
                int[] headerCount = { 2, 4, 6 };
                string[] headerName = { bankName, "Нақд пул ва бошқа қимматликларни қайта санаш натижалари бўйича НАЗОРАТ ҚАЙДНОМАСИ", "КИТОБИ" };

                ws.Cells["J1"].Value = "176-Шакл";
                ws.Cells["J1"].Style.Font.Bold = true;
                ws.Cells["J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < headerName.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < s.Length; i++)
                {
                    ws.Column(i + 2).Width = width[i];

                    ws.Cells[$"{s[i]}{a[i]}:{s[i]}{b[i]}"].Value = $"{c[i]}";
                    ws.Cells[$"{s[i]}{a[i]}:{s[i]}{b[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{s[i]}{a[i]}:{s[i]}{b[i]}"].Merge = true;
                    ws.Cells[$"{s[i]}{a[i]}:{s[i]}{b[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s[i]}{a[i]}:{s[i]}{b[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a[i]}", $"{s[i]}{b[i]}");
                }

                for (int i = 0; i < d1.Length; i++)
                {
                    ws.Row(9 + i).Height = 25;
                    ws.Column(i + 2).Width = width[i];
                    ws.Cells[$"{d1[i]}{DWidth[i]}:{d2[i]}{DWidth[i]}"].Value = $"{Dwords[i]}";
                    ws.Cells[$"{d1[i]}{DWidth[i]}:{d2[i]}{DWidth[i]}"].Merge = true;
                    ws.Cells[$"{d1[i]}{DWidth[i]}:{d2[i]}{DWidth[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{d1[i]}{DWidth[i]}:{d2[i]}{DWidth[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{d1[i]}{DWidth[i]}:{d2[i]}{DWidth[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ExcelAroundBorder.AroundBorder(ws, $"{d1[i]}{DWidth[i]}", $"{d2[i]}{DWidth[i]}");

                }

                // sanani joyi
                ws.Cells["J8"].Value = "Сана:";
                ws.Cells["J8"].Style.Font.Bold = true;
                ws.Cells["J8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                foreach (var item in model)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        ws.Cells[$"{s[i]}{a1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a1}");
                        ws.Cells[$"{s[i]}{a1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[$"{s[i]}{a1}"].Style.WrapText = true;
                    }
                    ws.Row(a1).Height = 30;
                    
                    ws.Cells[$"K8"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells[$"B{a1}"].Value = $"{item.BagNumber}";
                    ws.Cells[$"C{a1}"].Value = $"{item.CurrencyName}";
                    ws.Cells[$"D{a1}"].Value = $"{item.Summa:N}";
                    ws.Cells[$"E{a1}"].Value = $"{item.ExcessSumma:N}";
                    ws.Cells[$"F{a1}"].Value = $"{item.LackSumma:N}";
                    ws.Cells[$"G{a1}"].Value = $"{item.WorthlessSumma:N}";
                    ws.Cells[$"H{a1}"].Value = $"{item.ReceiptCount}";
                    ws.Cells[$"I{a1}"].Value = $"{item.ReceiptSumma:N}";
                    ws.Cells[$"J{a1}"].Value = $"{item.Comment}";
                    ws.Cells[$"K{a1}"].Value = $"{item.CounterCashierName}";
                    a1++;
                }

                ws.Cells[$"I{a1 + 1}"].Value = $"Кассир:";
                ws.Cells[$"J{a1 + 1}"].Value = $"{user}";
                ws.Cells[$"I{a1 + 2}"].Value = $"Бош Ҳисобчи:";
                ws.Cells[$"J{a1 + 2}"].Value = $"{model.FirstOrDefault().BoshKassir}";
                ws.Cells[$"I{a1 + 2}"].Style.Font.Bold = true;
                ws.Cells[$"I{a1 + 1}"].Style.Font.Bold = true;
                ws.Cells["B2:J2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:J4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:J6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
    public class Journal176Result
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Journal176ViewModels> Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; }
    }
}
