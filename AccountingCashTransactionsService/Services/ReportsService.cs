using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using ClosedXML.Excel;
using Entitys.DB;
using Entitys.Helper.ExcelAround;
using Entitys.Models;
using Entitys.Models.Auth;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation;
using Entitys.ViewModels.CashOperation.ReportModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class ReportsService : IReportsService
    {
        /// <summary>
        /// 
        /// </summary>
        DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ReportsService(DataContext context)
        {
            _context = context;
        }
        private static List<Rep2BottomPart> BottomPart = new List<Rep2BottomPart>();
        public ResponseCoreData CashOperByCashierRep1(DateTime fromDate, DateTime toDate, int bankCode)
        {
            try
            {
                var retResult = GetCashOperationByMain(fromDate, toDate, bankCode);

                return new ResponseCoreData(retResult, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        private CashOperByCashierRep1 GetCashOperationByMain(DateTime fromDate, DateTime toDate, int bankCode)
        {
            var retResult = new CashOperByCashierRep1();
            var model = new CashOperByCashierRep1Main();
            var cashOperByCashierRep1Bottom = new CashOperByCashierRep1Bottom();
            var result = new List<CashOperByCashierRep1Main>();
            double allSumma = 0;
            double allSummaBottom = 0;
            string sqltxt = $"select * from table(GET_REPORT1_CASHIERS(to_date('{DateToString(fromDate)}','DD.MM.YYYY'),to_date('{DateToString(toDate)}','DD.MM.YYYY'), {bankCode}))";
            var models = _context.Query<GetReport1Cashiers>().FromSql(sqltxt).ToList();
            int ii = 0;
            foreach (var item in models)
            {
                model.Id = item.CASHIERID;
                model.Fio = item.FIO;
                model.Position = item.POSITION;
                int i = 0;
                model.ValuesDate = new List<CashOperByCashierValue>();
                var cashOperByCashierValueListBottom = new List<CashOperByCashierValue>();
                for (DateTime dt = fromDate; dt <= toDate;)
                {
                    string sqltxt1 = $"select * from table(GET_REPORT1_SUMMS(to_date('{DateToString(dt)}','DD.MM.YYYY'), {bankCode}))";
                    var modelForSumma = _context.Query<GetReport1Summs>().FromSql(sqltxt1).ToList();
                    var summa = modelForSumma.FirstOrDefault(f => f.CashierId == item.CASHIERID)?.Summa;
                    var cashOperByCashierValue = new CashOperByCashierValue();
                    var cashOperByCashierValueBottom = new CashOperByCashierValue();
                    cashOperByCashierValue.Summa = summa ?? 0d;
                    cashOperByCashierValueBottom.Summa = summa ?? 0d;
                    allSummaBottom += summa ?? 0d;
                    model.ValuesDate.Add(cashOperByCashierValue);
                    cashOperByCashierValueListBottom.Add(cashOperByCashierValueBottom);

                    dt = dt.AddDays(1);
                    if (ii != 0)
                        cashOperByCashierRep1Bottom.Values[i].Summa += summa ?? 0d;
                    i++;
                }
                if (ii == 0)
                    cashOperByCashierRep1Bottom.Values = cashOperByCashierValueListBottom;
                ii++;
                model.AllSumma = allSumma;
                allSumma = 0;
                result.Add(model);
                model = new CashOperByCashierRep1Main();
            }
            cashOperByCashierRep1Bottom.AllSumma = allSummaBottom;
            retResult.MainPart = result;
            retResult.BottomPart = cashOperByCashierRep1Bottom;

            return retResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetBanks()
        {
            try
            {
                var banks = _context.Query<GetBankAll>().FromSql("select KOD as \"Id\", Name from table(GET_BANK_ALL)").ToList();
                return new ResponseCoreData(banks, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public ResponseCoreData GetCashOperationsByCashiers(DateTime fromDate, DateTime toDate, int bankCode)
        {
            try
            {
                var models = new List<ReportsViewModel>();
                int i = 0;
                for (DateTime dt = fromDate; dt <= toDate;)
                {
                    string sqltxt = $"select * from table(GET_REPORT1(to_date('{DateToString(dt)}','DD.MM.YYYY'),{bankCode}))";
                    var model = _context.Query<Reports>().FromSql(sqltxt).ToList().FirstOrDefault();
                    var reportModel = new ReportsViewModel();
                    reportModel.Date = DateToString(dt);
                    reportModel.IsUser = model.IsUser;
                    reportModel.Kod = model.Kod;
                    reportModel.Name = model.Name;
                    reportModel.Summa = model.Summa;
                    reportModel.Fio = model.Fio;
                    if (model != null)
                        models.Add(reportModel);

                    dt = dt.AddDays(1);
                }
                return new ResponseCoreData(models, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public ResponseCoreData GetReport1Val(DateTime fromDate, DateTime toDate, int bankCode)
        {
            try
            {
                var models = new List<ReportsViewModel>();
                var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
                for (DateTime dt = fromDate; dt <= toDate;)
                {

                    foreach (var item in currencyTypes)
                    {
                        string sqltxt = $"select * from table(GET_REPORT1_VAL(to_date('{DateToString(dt)}','DD.MM.YYYY'), {bankCode}, {item.Kod}))";
                        var model = _context.Query<Reports>().FromSql(sqltxt).ToList().FirstOrDefault();
                        var reportModel = new ReportsViewModel();
                        reportModel.Date = DateToString(dt);
                        reportModel.IsUser = model.IsUser;
                        reportModel.Kod = model.Kod;
                        reportModel.Name = model.Name;
                        reportModel.Summa = model.Summa;
                        reportModel.Fio = model.Fio;
                        if (model != null)
                            models.Add(reportModel);
                    }
                    dt = dt.AddDays(1);
                }

                return new ResponseCoreData(models, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ResponseCoreData GetReport2Val(DateTime fromDate, DateTime toDate, int bankCode)
        {
            try
            {
                BottomPart = new List<Rep2BottomPart>();
                var rep2MainPartExemplar = new Rep2MainPartExemplar();
                var result = new Report2ViewModelExemplar();
                string sql = $"select * from table(GET_REPORT1_CURRENCY_CASHIERS(to_date('{DateToString(fromDate)}','DD.MM.YYYY'),to_date('{DateToString(toDate)}','DD.MM.YYYY'),{bankCode}))";
                var rep2CurrencyCashier = _context.Query<GetReport2CurrencyCashiers>().FromSql(sql).ToList();
                var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
                string sql1 = $"select * from table(GET_REPORT1_CURRENCY(to_date('{DateToString(fromDate)}','DD.MM.YYYY'),to_date('{DateToString(toDate)}','DD.MM.YYYY'),{bankCode}))";
                var getReport2Currency = _context.Query<GetReport2Currency>().FromSql(sql1).ToList();
                result.MainPart = new List<Rep2MainPartExemplar>();
                var mainPart = new List<Rep2MainPartExemplar>();
                foreach (var item in rep2CurrencyCashier)
                {
                    var resurs = Rep2MainPartExemplarView(item, getReport2Currency.Where(f => f.CashierId == item.CashierId).ToList());
                    result.MainPart.Add(resurs);
                }
                mainPart = result.MainPart;
                for (DateTime dt = fromDate; dt <= toDate;)
                {
                    string sql2 = $"select * from table(GET_REPORT1_CURRENCY_SUMMS(to_date('{DateToString(dt)}','DD.MM.YYYY'), {bankCode}))";
                    var getReport2CurrencySumms = _context.Query<GetReport2CurrencySumms>().FromSql(sql2).ToList();
                    mainPart = GetSummaWithDate(mainPart, getReport2CurrencySumms, dt);
                    dt = dt.AddDays(1);
                }
                mainPart.ForEach(f => f.Currency.ForEach(x => { x.AllSumma = x.ValuesDate.Sum(a => a.Summa); }));
                BottomPart.ForEach(f => f.AllValues = f.ValuesDate.Sum(a => a.Summa));
                result.MainPart = mainPart;
                result.BottomPart = BottomPart;
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private List<CashOperByCashierValue> GetValues(List<GetReport2CurrencySumms> getReport2CurrencySumms, int cashierId, int kod, DateTime date)
        {
            var rep = getReport2CurrencySumms.Where(w => w.CashierId == cashierId && w.CurrencyId == kod).ToList();
            return rep.Select(f => ConvertToValue(f, date)).ToList();
        }

        private CashOperByCashierValue ConvertToValue(GetReport2CurrencySumms model, DateTime date)
        {
            var result = new CashOperByCashierValue();
            result.Summa = model.Summa ?? 0;
            return result;
        }

        public FileContentResult TestExcelSimpleReport()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Cash Book Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Cashier name";
                worksheet.Column(1).Width = 30;
                worksheet.Column(2).Width = 50;


                worksheet.Rows().AdjustToContents();
                var users = new List<string>();
                users.Add("main cashier");
                users.Add("abc jlkjlkjlkj");
                users.Add("abc sdjlskjdl sldkjslkdj");
                users.Add("abcjqlk qwlkjelkwqe");
                users.Add("abc5");
                users.Add("abc ejlkqwje lwqkje");
                users.Add("abc qwepoqwie");
                users.Add("abc qoepipoei");
                users.Add("abc qepo-039");
                users.Add("abc ;led;lkdlksdwe");
                users.Add("Repl cashier");

                foreach (var user in users)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                    worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(currentRow, 2).Style.Alignment.WrapText = true;

                    worksheet.Cell(currentRow, 2).Value = user;
                    //worksheet.Cell(currentRow, 2).Style.Font.FontSize = 20;
                    //worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.Alizarin;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var result = new FileContentResult(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.FileDownloadName = "CashBookTestReport";
                    return result;
                }
            }
        }
        private string DateToString(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }
        public ResponseCoreData GetReport2ValExemplar(DateTime fromDate, DateTime toDate, int bankCode)
        {
            try
            {
                var rep2MainPartExemplar = new Rep2MainPartExemplar();
                var result = new Report2ViewModelExemplar();
                string sql = $"select * from table(GET_REPORT1_CURRENCY_CASHIERS(to_date('{DateToString(fromDate)}','DD.MM.YYYY'),to_date('{DateToString(toDate)}','DD.MM.YYYY'),{bankCode}))";
                var rep2CurrencyCashier = _context.Query<GetReport2CurrencyCashiers>().FromSql(sql).ToList();
                var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
                string sql1 = $"select * from table(GET_REPORT1_CURRENCY(to_date('{DateToString(fromDate)}','DD.MM.YYYY'),to_date('{DateToString(toDate)}','DD.MM.YYYY'),{bankCode}))";
                var getReport2Currency = _context.Query<GetReport2Currency>().FromSql(sql1).ToList();
                result.MainPart = new List<Rep2MainPartExemplar>();
                var mainPart = new List<Rep2MainPartExemplar>();
                foreach (var item in rep2CurrencyCashier)
                {
                    var resurs = Rep2MainPartExemplarView(item, getReport2Currency.Where(f => f.CashierId == item.CashierId).ToList());
                    result.MainPart.Add(resurs);
                }
                mainPart = result.MainPart;
                for (DateTime dt = fromDate; dt <= toDate;)
                {
                    string sql2 = $"select * from table(GET_REPORT1_CURRENCY_SUMMS(to_date('{DateToString(dt)}','DD.MM.YYYY'), {bankCode}))";
                    var getReport2CurrencySumms = _context.Query<GetReport2CurrencySumms>().FromSql(sql2).ToList();
                    mainPart = GetSummaWithDate(mainPart, getReport2CurrencySumms, dt);
                    dt = dt.AddDays(1);
                }

                result.MainPart = mainPart;
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private List<Rep2MainPartExemplar> GetSummaWithDate(List<Rep2MainPartExemplar> mainPart, List<GetReport2CurrencySumms> getReport2CurrencySumms, DateTime dt)
        {
            bool add = false;
            var bottomList = new List<Rep2BottomPart>();
            foreach (var item1 in mainPart)
            {
                foreach (var item2 in item1.Currency)
                {
                    var summ = getReport2CurrencySumms.FirstOrDefault(f => f.CashierId == item1.CashierId && f.CurrencyId == item2.CurrencyId);
                    if (summ != null)
                    {
                        var bottom = bottomList.FirstOrDefault(f => f.CurrencyId == item2.CurrencyId);
                        if (bottom != null)
                        {
                            var datebottom = bottom.ValuesDate.FirstOrDefault(f => f.Date == dt);
                            if (datebottom != null)
                            {
                                datebottom.Summa += summ.Summa.Value;
                                add = true;

                            }
                            else
                            {
                                bottom.ValuesDate = new List<CashOperByCashierValueBottom>();
                                var cash = new CashOperByCashierValueBottom();
                                cash.Date = dt;
                                cash.Summa = summ.Summa.Value;
                            }
                        }
                        else
                        {
                            bottom = new Rep2BottomPart();
                            bottom.CurrencyId = item2.CurrencyId;
                            bottom.CurrencyName = item2.CurrencyName;
                            bottom.ValuesDate = new List<CashOperByCashierValueBottom>();
                            var cash = new CashOperByCashierValueBottom();
                            cash.Date = dt;
                            cash.Summa = summ.Summa.Value;
                            bottom.ValuesDate.Add(cash);
                            add = false;
                        }
                        if (add == false)
                            bottomList.Add(bottom);

                        item2.ValuesDate.Add(new CashOperByCashierValue { Summa = summ.Summa.Value });

                    }
                    else
                    {
                        item2.ValuesDate.Add(new CashOperByCashierValue { Summa = 0d });
                        item2.AllSumma += 0d;
                    }

                }
            }
            if (BottomPart.Count() != 0)
            {
                foreach (var item in BottomPart)
                {
                    item.ValuesDate.AddRange(bottomList.FirstOrDefault(f => f.CurrencyId == item.CurrencyId).ValuesDate);
                }
            }
            else
            {
                BottomPart = bottomList;
            }



            return mainPart;
        }

        private Rep2MainPartExemplar Rep2MainPartExemplarView(GetReport2CurrencyCashiers model, List<GetReport2Currency> getReport2Currency)
        {

            var result = new Rep2MainPartExemplar();
            result.Currency = new List<Currency>();
            result.CashierId = model.CashierId;
            result.Fio = model.Fio;
            result.Position = model.Position;
            result.IsUser = TryParse(model.Isuser);
            result.Currency.AddRange(GetReport2CurrencyView(getReport2Currency));
            return result;
        }
        private bool TryParse(int a)
        {
            if (a == 1)
                return true;
            else
                return false;
        }
        private int GetDay(DateTime fromDate, DateTime toDate)
        {
            var a = (toDate - fromDate).TotalDays;
            int b = Convert.ToInt32(a);
            return b;
        }
        private List<Currency> GetReport2CurrencyView(List<GetReport2Currency> model)
        {
            var currencys = new List<Currency>();
            var currency = new Currency();
            foreach (var item in model)
            {
                currency.CurrencyId = item.CurrencyId;
                currency.CurrencyName = item.CurrencyName;
                currency.ValuesDate = new List<CashOperByCashierValue>();
                currencys.Add(currency);
                currency = new Currency();
            }
            return currencys;
        }

        public byte[] ToExport(Report2ExcelExport model)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                int row = 5;
                int col = 4;
                int oldcol = 4;
                var excel = new ExcelPackage();
                string[] head = { "\"Ипотека банк\" АТИБ ______________ филиали бўйича касса амалиётлари кассирлари томонидан кунлик санаб, ўраб, боғланган нақд пуллар тўғрисида", "МАЪЛУМОТ" };
                string[] headcoll = { "A1", "A2" };
                string[] headrow = { "R1", "R2" };
                var ws = excel.Workbook.Worksheets.Add("Reports");
                for (int i = 0; i < head.Length; i++)
                {
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Value = $"{head[i]}";
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Merge = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.WrapText = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                int a = 4;
                int[] columnId1 = { a, a, a + 1, a + 1 };
                int[] columnId2 = { a + 1, a, a + 1, a + 1 };
                string[] c = { "№", "Кассир", "Ф.И.О.", "Лавозими(кирим, чиқим, қайта санаш)" };
                string ca = "";
                string[] columnLetter1 = { "A", "B", "B", "C" };

                string[] columnLetter2 = { "A", "C", "B", "C" };

                int[] columnWith = { 10, 20, 30, 20, 30, 25, 20, 10, 20, 10, 10, 10, 10, 10 };

                for (int i = 0; i < c.Length; i++)
                {
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.WrapText = true;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Value = $"{c[i]}";
                    ws.Column(i + 1).Width = columnWith[i];
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Merge = true;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{columnLetter1[i]}{columnId1[i]}", $"{columnLetter2[i]}{columnId2[i]}");
                }
                for (DateTime dt = model.FromDate; dt <= model.ToDate;)
                {
                    ws.Cells[row, col].Value = $"{dt:dd.MM.yyyy}";
                    ws.Column(col + 1).Width = 20;
                    ws.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[row, col].Style.Font.Bold = true;
                    ExcelAroundBorder.AroundBorder(ws, row, col);
                    col++;
                    dt = dt.AddDays(1);
                }
                ws.Cells[row - 1, oldcol, row - 1, col - 1].Value = $"Сана";
                ws.Cells[row - 1, oldcol, row - 1, col - 1].Merge = true;
                ws.Cells[row - 1, oldcol, row - 1, col - 1].Style.Font.Bold = true;
                ws.Cells[row - 1, oldcol, row - 1, col - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[row - 1, oldcol, row - 1, col - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ExcelAroundBorder.AroundBorder(ws, row - 1, oldcol, row - 1, col - 1);
                var models = GetCashOperationByMain(model.FromDate, model.ToDate, model.BankKod);
                col = 1;
                int no = 1;
                row++;
                if (models != null)
                {
                    foreach (var item in models.MainPart)
                    {
                        ws.Cells[row, col].Value = no;
                        ws.Cells[row, (col + 1)].Value = item.Fio;
                        ws.Cells[row, (col + 2)].Value = item.Position;
                        ws.Cells[row, (col + 2)].Style.WrapText = true;

                        for (int j = 0; j < c.Length; j++)
                            ExcelAroundBorder.AroundBorder(ws, row, col + j);

                        col = 4;
                        for (int i = 0; i < item.ValuesDate.Count(); i++)
                        {
                            ws.Cells[row, col + i].Value = item.ValuesDate[i].Summa;
                            ws.Cells[row, col + i].Style.WrapText = true;
                            ExcelAroundBorder.AroundBorder(ws, row, col + i);
                        }
                        row++;
                        col = 1;
                        no++;
                    }
                }
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
