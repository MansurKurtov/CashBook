using AccountingCashTransactionsService.Helper;
using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Enums;
using Entitys.Helper.ExcelAround;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation;
using Entitys.ViewModels.CashOperation.Journal15;
using Entitys.ViewModels.CashOperation.Journal16VM;
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
    public class Journal15Service : EntityRepositoryCore<Journal15>, IJournal15Service
    {
        private CommonHelper _commonHelper;
        private DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexts"></param>
        /// <param name="context"></param>
        public Journal15Service(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
            _commonHelper = new CommonHelper(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Journal15 ToEntity(Journal15ViewModel model)
        {
            var result = new Journal15();
            result.Id = model.Id;
            result.Journal16Id = model.Journal16Id;
            result.Summa = model.Summa;
            result.Description = model.Description;
            result.UserId = model.UserId;
            result.BagsNumber = model.BagsNumber;
            result.IsEmpty = model.IsEmpty;
            result.SprObjectId = model.SprObjectId;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Journal15ViewModel ToModel(Journal15 entity)
        {
            var result = new Journal15ViewModel();
            result.Id = entity.Id;
            result.Journal16Id = entity.Journal16Id;
            result.Summa = entity.Summa;
            result.Description = entity.Description;
            result.UserId = entity.UserId;
            result.BagsNumber = entity.BagsNumber;
            result.IsEmpty = entity.IsEmpty;
            result.SystemDate = entity.SystemDate;
            result.SprObjectId = entity.SprObjectId;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Add(int bankCode, int userId, Journal15ViewModel model)
        {
            var entity = ToEntity(model);
            entity.SystemDate = DateTime.Now;
            Add(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal15, EventType.Edit);

            return new ResponseCoreData(entity, ResponseStatusCode.OK);
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
            var result = new Journal15ResultViewModel();
            var findResult = Find(f => f.Journal16Id == journal16Id).Skip(skip * take).Take(take).ToList();
            result.Data = findResult.Select(ToModel).ToList();
            result.Total = findResult.Count;
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
            var entity = _context.Journal15s.Where(f => f.Id == Id).ToList().FirstOrDefault();
            if (entity == null)
                return new ResponseCoreData(new Exception("Not Found"));
            var model = ToModel(entity);

            return new ResponseCoreData(model, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Update(int bankCode, int userId, Journal15ViewModel model)
        {
            var existsEntity = _context.Journal15s.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
            if (existsEntity == null)
                return new ResponseCoreData(new Exception("Not Found"));
            existsEntity.Id = model.Id;
            existsEntity.Journal16Id = model.Journal16Id;
            existsEntity.Summa = model.Summa;
            existsEntity.Description = model.Description;
            existsEntity.UserId = model.UserId;
            existsEntity.BagsNumber = model.BagsNumber;
            existsEntity.IsEmpty = model.IsEmpty;
            existsEntity.SystemDate = model.SystemDate;

            _context.Journal15s.Update(existsEntity);
            _context.SaveChanges();

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal15, EventType.Edit);

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
            var entity = _context.Journal15s.Where(f => f.Id == Id).ToList().FirstOrDefault();
            if (entity == null)
                return new ResponseCoreData(new Exception("Not Found"));

            Delete(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal15, EventType.Delete);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData IsDayClosed(int bankCode, DateTime date)
        {
            var commonHelper = new CommonHelper(_context);
            var result = new DayClosedViewModel();
            result.IsDayClosed = commonHelper.IsDayClosed((int)ModuleType.Journal15, bankCode, date);
            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        private string GetBankName(int bankId)
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

        public ResponseCoreData GetCurrencyList()
        {
            var result = new List<GetValType>();
            result.Add(new GetValType() { Kod = 0, Name = "Сум" });
            var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
            result.AddRange(currencyTypes);

            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        private string GetCurrencyTypeById(int id)
        {
            var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
            var result = currencyTypes.Where(w => w.Kod == id).FirstOrDefault();
            if (result?.Name == null)
                return "Сўм";

            return result?.Name;
        }

        private List<Journal15CurrencyViewModel> GetJournal15CurrencyInternational(int id)
        {
            var currencyes = _context.Journal15s.Where(w => w.Journal16Id == id && w.SprObjectId!=0).GroupBy(g => g.SprObjectId).
                Select(s => new Journal15CurrencyViewModel()
                {
                    Kod = s.Key,
                    Name = string.Empty,
                    Value = s.Sum(x => x.Summa),
                    Count = s.Count()
                }).ToList();

            currencyes.ForEach(f => f.Name = GetCurrencyTypeById(f.Kod));

            return currencyes;
        }


        private List<Journal15CurrencyViewModel> GetJournal15CurrencySum(int id)
        {
            var currencyes = _context.Journal15s.Where(w => w.Journal16Id == id && w.SprObjectId == 0).GroupBy(g => g.SprObjectId).
                Select(s => new Journal15CurrencyViewModel()
                {
                    Kod = s.Key,
                    Name = string.Empty,
                    Value = s.Sum(x => x.Summa),
                    Count = s.Count()
                }).ToList();

            currencyes.ForEach(f => f.Name = GetCurrencyTypeById(f.Kod));

            return currencyes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public byte[] ToExport(Journal15ForEcxel model, string user, int bankCode)
        {
            var bankName = GetBankName(bankCode);
            var summaryInfos = GetJournal15CurrencyInternational(model.Journal16Id);

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                double summa = 0;
                var models = _context.Journal15s.Where(f => f.Journal16Id == model.Journal16Id && f.SprObjectId != 0).ToList();
                var ws = excel.Workbook.Worksheets.Add("Journal15");
                string[] headerName = { bankName, "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА ", "КИТОБИ ", $"{model.DirectionNumber}-сонли йўналиш (ташриф)", "Банк кассаси кассирлари томонидан 30 июн 2020 йил соат ", "10:09 да инкассаторлар гуруҳи аъзоларидан", "Банк кассасига қабул қилинган инкассация сумкасининг (қопининг)" };
                int[] headerCount = { 2, 3, 4, 5, 6, 7, 8 };
                int a = 9;
                string[] s = { "C", "D", "E", "F" };
                string[] c = { "Боғлам(қоп) рақами", "Валюта", "Сумма", "Изоҳлар" };

                ws.Cells["E1"].Value = "15-Шакл";
                ws.Cells["E1"].Style.Font.Bold = true;
                ws.Cells["E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["B8:B9"].Value = "T/p";
                ws.Cells["B8:B9"].Merge = true;
                ws.Cells["B8:B9"].Style.Font.Bold = true;
                ws.Cells["B8:B9"].Style.WrapText = true;
                ws.Cells["B8:B9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ExcelAroundBorder.AroundBorder(ws, $"B8", "B9");
                ExcelAroundBorder.AroundBorder(ws, $"B12", "B13");
                ExcelAroundBorder.AroundBorder(ws, $"C12", "E12");

                for (int i = 0; i < s.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Column(i + 3).Width = 30;
                    ws.Cells[$"{s[i]}{a}"].Value = $"{c[i]}";
                    ws.Cells[$"{s[i]}{a}"].Merge = true;
                    ws.Cells[$"{s[i]}{a}"].Style.Font.Bold = true;
                    ws.Cells[$"{s[i]}{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s[i]}{a}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{s[i]}{a}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");
                }
                ws.Column(2).Width = 10;

                foreach (var item in models)
                {
                    a++;
                    for (int i = 0; i < s.Length; i++)
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");

                    ws.Cells[$"B{a}"].Value = $"{a - 9}";
                    ws.Cells[$"C{a}"].Value = $"{item.BagsNumber}";
                    ws.Cells[$"D{a}"].Value = $"{GetCurrencyTypeById(item.SprObjectId)}";
                    ws.Cells[$"E{a}"].Value = $"{item.Summa:N}";
                    ws.Cells[$"F{a}"].Value = $"{item.Description}";

                    ExcelAroundBorder.AroundBorder(ws, $"B{a}");
                    summa = summa + item.Summa;

                }

                var j = 1;
                summaryInfos.ForEach(f =>
                {
                    ws.Cells[$"C{a + j}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[$"C{a + j}"].Value = $"{f.Count}";
                    ws.Cells[$"C{a + j}"].Merge = true;
                    ws.Cells[$"C{a + j}"].Style.Font.Bold = true;
                    ExcelAroundBorder.AroundBorder(ws, $"C{a + j}");

                    ws.Cells[$"D{a + j}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[$"D{a + j}"].Value = $"{f.Name}";
                    ws.Cells[$"D{a + j}"].Merge = true;
                    ws.Cells[$"D{a + j}"].Style.Font.Bold = true;
                    ExcelAroundBorder.AroundBorder(ws, $"D{a + j}");
                    ws.Cells[$"E{a + j}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[$"E{a + j}"].Value = $"{f.Value}";
                    ws.Cells[$"E{a + j}"].Merge = true;
                    ws.Cells[$"E{a + j}"].Style.Font.Bold = true;
                    ExcelAroundBorder.AroundBorder(ws, $"E{a + j}");
                    ExcelAroundBorder.AroundBorder(ws, $"F{a + j}");
                    j++;
                });

                ws.Cells[$"B{a + 1}:B{a + j - 1}"].Value = "Жами:";
                ws.Cells[$"B{a + 1}:B{a + j - 1}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"B{a + 1}:B{a + j - 1}");
                ws.Cells[$"B{a + 1}:B{a + j - 1}"].Merge = true;
                a += j++;
                ws.Cells[$"B{a + 1}:C{a + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ws.Cells[$"B{a + 3}:E{a + 3}"].Value = $"Ушбу йўналиш (ташриф) бўйича инкассация сумка (қоп)ларининг юк хатларида қайд этилган умумий суммаси {model.Summa} ({ConverterWord.SumInWord(model.Summa)}) сўм бўлган пломбаланган {model.BagsCount} та сумка (қоп)лар банк (филиал) кассасига қабул қилинди. ";
                ws.Cells[$"B{a + 3}:E{a + 3}"].Style.WrapText = true;
                ws.Cells[$"B{a + 3}:E{a + 3}"].Merge = true;
                ws.Cells[$"B{a + 4}:E{a + 4}"].Value = $"суммаси {model.Summa} ({ConverterWord.SumInWord(model.Summa)}) сўм бўлган пломбаланган {model.BagsCount} та сумка (қоп)лар банк (филиал) кассасига қабул қилинди. ";
                ws.Cells[$"B{a + 4}:E{a + 4}"].Style.WrapText = true;
                ws.Cells[$"B{a + 4}:E{a + 4}"].Merge = true;

                ws.Cells[$"B{a + 6}:E{a + 6}"].Value = $"Инкассаторлар гуруҳи топширган тушум пуллари солинган сумка (қоп)лар рақамлари ва улар сони, ташриф ";
                ws.Cells[$"B{a + 6}:E{a + 6}"].Style.WrapText = true;
                ws.Cells[$"B{a + 6}:E{a + 6}"].Merge = true;
                ws.Cells[$"B{a + 7}:E{a + 7}"].Value = $"варақаларида, шунингдек, инкассация сумка(қоп)лари юк хатларида кўрсатилган ёзувларга мос келди.";
                ws.Cells[$"B{a + 7}:E{a + 7}"].Style.WrapText = true;
                ws.Cells[$"B{a + 7}:E{a + 7}"].Merge = true;
                ws.Cells[$"B{a + 8}:E{a + 8}"].Value = $"Ташриф варақаси ва юк хатларида кўрсатилган тушум суммалари мазкур журналда тўғри қайд этилганлигини тасдиқлаймиз. ";
                ws.Cells[$"B{a + 8}:E{a + 8}"].Style.WrapText = true;
                ws.Cells[$"B{a + 8}:E{a + 8}"].Merge = true;
                ws.Cells["A2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public byte[] ToExportSum(Journal15ForEcxel model, string user, int bankCode)
        {
            var bankName = GetBankName(bankCode);
            var summaryInfos = GetJournal15CurrencySum(model.Journal16Id);

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                double summa = 0;
                var models = _context.Journal15s.Where(f => f.Journal16Id == model.Journal16Id && f.SprObjectId != 0).ToList();
                var ws = excel.Workbook.Worksheets.Add("Journal15");
                string[] headerName = { bankName, "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА ", "КИТОБИ ", $"{model.DirectionNumber}-сонли йўналиш (ташриф)", "Банк кассаси кассирлари томонидан 30 июн 2020 йил соат ", "10:09 да инкассаторлар гуруҳи аъзоларидан", "Банк кассасига қабул қилинган инкассация сумкасининг (қопининг)" };
                int[] headerCount = { 2, 3, 4, 5, 6, 7, 8 };
                int a = 9;
                string[] s = { "C", "D", "E"};
                string[] c = { "Боғлам(қоп) рақами", "Сумма", "Изоҳлар" };

                ws.Cells["E1"].Value = "15-Шакл";
                ws.Cells["E1"].Style.Font.Bold = true;
                ws.Cells["E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["B8:B9"].Value = "T/p";
                ws.Cells["B8:B9"].Merge = true;
                ws.Cells["B8:B9"].Style.Font.Bold = true;
                ws.Cells["B8:B9"].Style.WrapText = true;
                ws.Cells["B8:B9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ExcelAroundBorder.AroundBorder(ws, $"B8", "B9");
                ExcelAroundBorder.AroundBorder(ws, $"B12", "B13");
                ExcelAroundBorder.AroundBorder(ws, $"C12", "E12");

                for (int i = 0; i < s.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Column(i + 3).Width = 30;
                    ws.Cells[$"{s[i]}{a}"].Value = $"{c[i]}";
                    ws.Cells[$"{s[i]}{a}"].Merge = true;
                    ws.Cells[$"{s[i]}{a}"].Style.Font.Bold = true;
                    ws.Cells[$"{s[i]}{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s[i]}{a}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{s[i]}{a}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");
                }
                ws.Column(2).Width = 10;

                foreach (var item in models)
                {
                    a++;
                    for (int i = 0; i < s.Length; i++)
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");

                    ws.Cells[$"B{a}"].Value = $"{a - 9}";
                    ws.Cells[$"C{a}"].Value = $"{item.BagsNumber}";
                    //ws.Cells[$"D{a}"].Value = $"{GetCurrencyTypeById(item.SprObjectId)}";
                    ws.Cells[$"D{a}"].Value = $"{item.Summa:N}";
                    ws.Cells[$"E{a}"].Value = $"{item.Description}";

                    ExcelAroundBorder.AroundBorder(ws, $"B{a}");
                    summa = summa + item.Summa;
                }

                ws.Cells[$"B{a + 1}:B{a + 1}"].Value = "Жами:";
                ws.Cells[$"B{a + 1}:B{a + 1}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"B{a + 1}");
                ws.Cells[$"B{a + 1}:B{a + 1}"].Merge = true;

                ws.Cells[$"C{a + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"C{a + 1}"].Value = $"{model.BagsCount}";
                ws.Cells[$"C{a + 1}"].Merge = true;
                ws.Cells[$"C{a + 1}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"C{a + 1}");
                ws.Cells[$"D{a + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"D{a + 1}"].Value = $"{summa}";
                ws.Cells[$"D{a + 1}"].Merge = true;
                ws.Cells[$"D{a + 1}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"D{a + 1}");
                ExcelAroundBorder.AroundBorder(ws, $"E{a + 1}");

                ws.Cells[$"B{a + 1}:C{a + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ws.Cells[$"B{a + 3}:E{a + 3}"].Value = $"Ушбу йўналиш (ташриф) бўйича инкассация сумка (қоп)ларининг юк хатларида қайд этилган умумий суммаси {model.Summa} ({ConverterWord.SumInWord(model.Summa)}) сўм бўлган пломбаланган {model.BagsCount} та сумка (қоп)лар банк (филиал) кассасига қабул қилинди. ";
                ws.Cells[$"B{a + 3}:E{a + 3}"].Style.WrapText = true;
                ws.Cells[$"B{a + 3}:E{a + 3}"].Merge = true;
                ws.Cells[$"B{a + 4}:E{a + 4}"].Value = $"суммаси {model.Summa} ({ConverterWord.SumInWord(model.Summa)}) сўм бўлган пломбаланган {model.BagsCount} та сумка (қоп)лар банк (филиал) кассасига қабул қилинди. ";
                ws.Cells[$"B{a + 4}:E{a + 4}"].Style.WrapText = true;
                ws.Cells[$"B{a + 4}:E{a + 4}"].Merge = true;

                ws.Cells[$"B{a + 6}:E{a + 6}"].Value = $"Инкассаторлар гуруҳи топширган тушум пуллари солинган сумка (қоп)лар рақамлари ва улар сони, ташриф ";
                ws.Cells[$"B{a + 6}:E{a + 6}"].Style.WrapText = true;
                ws.Cells[$"B{a + 6}:E{a + 6}"].Merge = true;
                ws.Cells[$"B{a + 7}:E{a + 7}"].Value = $"варақаларида, шунингдек, инкассация сумка(қоп)лари юк хатларида кўрсатилган ёзувларга мос келди.";
                ws.Cells[$"B{a + 7}:E{a + 7}"].Style.WrapText = true;
                ws.Cells[$"B{a + 7}:E{a + 7}"].Merge = true;
                ws.Cells[$"B{a + 8}:E{a + 8}"].Value = $"Ташриф варақаси ва юк хатларида кўрсатилган тушум суммалари мазкур журналда тўғри қайд этилганлигини тасдиқлаймиз. ";
                ws.Cells[$"B{a + 8}:E{a + 8}"].Style.WrapText = true;
                ws.Cells[$"B{a + 8}:E{a + 8}"].Merge = true;
                ws.Cells["A2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
