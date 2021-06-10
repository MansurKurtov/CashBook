using AccountingCashTransactionsService.Helper;
using AccountingCashTransactionsService.Interfaces;
using AuthService.Enums;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Enums;
using Entitys.Helper.ExcelAround;
using Entitys.Helper.Unicode;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation;
using Entitys.ViewModels.CashOperation.Journal109ViewModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class Book171Service : EntityRepositoryCore<Book171>, IBook171Service
    {
        private CommonHelper _commonHelper;
        private DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexts"></param>
        /// <param name="context"></param>
        public Book171Service(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
            _commonHelper = new CommonHelper(context);
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
                var entity = Find(f => f.Id != 0).ToList();

                //_commonHelper.SaveUserEvent(userId, ModuleType.Book171, EventType.Read);

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
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(int bankCode, int userId, DateTime date)
        {
            try
            {
                var result = new Journal171ResultQuery();
                var dateString = date.ToString("dd.MM.yyyy");
                string sqltxt = String.Format($"select * from table(GET_BOOK_171(to_date('{dateString}','DD.MM.YYYY'),{bankCode}))");
                var entity = _context.Query<Book171QueryModel>().FromSql(sqltxt).ToList();
                foreach (var item in entity)
                {
                    item.Name = UnicodeTo.unicodeToString(item.Name);
                }
                result.Data = entity;
                result.Total = entity.Count();

                //_commonHelper.SaveUserEvent(userId, ModuleType.Book171, EventType.Read);

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
        /// <param name="permissions"></param>
        /// <param name="date"></param>
        /// <param name="accountCode"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        public ResponseCoreData SetFirstSaldo(int bankCode, string permissions, DateTime date, string accountCode, int saldoBeginCount, double saldoBeginSumma)
        {
            if (!permissions.Contains(Permission.CashBookProcedureRun))
                return new ResponseCoreData("Нет доступ", ResponseStatusCode.BadRequest);
            try
            {
                var insertedRowsCount = SetSaldoBeginProcedure(bankCode, date, accountCode, saldoBeginCount, saldoBeginSumma);
                return new ResponseCoreData(insertedRowsCount, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ResponseCoreData GetWorthAccounts(int orgId)
        {
            try
            {
                var worths = _context.Query<GetWorthAll>().FromSql($"select * from table(GET_WORTH_ALL({orgId}))").ToList();
                return new ResponseCoreData(worths, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="accountCode"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        private int SetSaldoBeginProcedure(int bankCode, DateTime date, string accountCode, int saldoBeginCount, double saldoBeginSumma)
        {
            int insertedRowsCount;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SET_FIRST_SALDO_BOOK171";

                var dateParam = new OracleParameter("SANA", OracleDbType.Date, ParameterDirection.Input);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);

                var bankCodeParam = new OracleParameter("BANKKOD", OracleDbType.Int32, ParameterDirection.Input);
                bankCodeParam.Value = bankCode;
                cmd.Parameters.Add(bankCodeParam);

                var accountCodeParam = new OracleParameter("KODACCOUNT", OracleDbType.NVarchar2, ParameterDirection.Input);
                accountCodeParam.Value = accountCode;
                cmd.Parameters.Add(accountCodeParam);

                var saldoBeginCountParam = new OracleParameter("SALDO_BEGINCOUNT", OracleDbType.Int32, ParameterDirection.Input);
                saldoBeginCountParam.Value = saldoBeginCount;
                cmd.Parameters.Add(saldoBeginCountParam);

                var saldoBeginSummaParam = new OracleParameter("SALDO_BEGINSUMMA", OracleDbType.Double, ParameterDirection.Input);
                saldoBeginSummaParam.Value = saldoBeginSumma;
                cmd.Parameters.Add(saldoBeginSummaParam);

                var rowsCountParam = new OracleParameter("RESULT_ROWS", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(rowsCountParam);

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                DataTable budt = new DataTable();
                while (reader.Read())
                {
                    budt.Load(reader);
                }
                insertedRowsCount = int.Parse(rowsCountParam.Value.ToString());
                cmd.Connection.Close();
            }

            return insertedRowsCount;
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
        public byte[] ToExport(List<Book171Excel> model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();

                var ws = excel.Workbook.Worksheets.Add("Book171");
                int[] a = { 9, 9, 9, 9, 9, 9 };
                int[] b = { 10, 10, 10, 9, 9, 10 };
                string[] s = { "B", "C", "D", "E", "G", "I" };
                string[] d = { "B", "C", "D", "F", "H", "I" };
                string[] c = { "Ҳисоб", "Валюта номи", "Кун бошига сальдо", "Кирим", "Чиқим", "Кун охирига сальдо" };
                string[] headerName = { bankName, "ПУЛ ОМБОРИДАГИ ҚАТЪИЙ ҲИСОБДА ТУРУВЧИ БЛАНКЛАР ҲАМДА САҚЛАНАЁТГАН ҚИММАТЛИКЛАР ҚОЛДИҒИНИ ҚАЙД ЭТИШ КИТОБИ ", "КИТОБИ " };
                int[] headerCount = { 2, 4, 6 };
                int column = 11;
                int col = 0;
                int a1 = 12;
                int count = 0;
                string[] s1 = { "B", "C", "D", "E", "F", "G", "H", "I" };
                string[] s2 = { "D", "E", "F", "G", "H", "I" };
                var book171model = new Book171Excel();

                ws.Cells["I1"].Value = "171-Шакл";
                ws.Cells["I1"].Style.Font.Bold = true;
                ws.Cells["I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < headerName.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < s.Length; i++)
                {
                    ws.Column(i + 2).Width = 20;
                    ws.Column(i + 4).Width = 20;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Value = $"{c[i]}";
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a[i]}", $"{d[i]}{b[i]}");
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Merge = true;
                    if (s[i] == "E" || s[i] == "G")
                    {
                        ws.Cells[$"{s[i]}{a[i] + 1}"].Value = "Ҳужжатлар сони";
                        ws.Cells[$"{d[i]}{b[i] + 1}"].Value = "Сумма ";
                        ws.Cells[$"{s[i]}{a[i] + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[$"{d[i]}{b[i] + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[$"{s[i]}{a[i] + 1}"].Style.Font.Bold = true;
                        ws.Cells[$"{d[i]}{b[i] + 1}"].Style.Font.Bold = true;
                        ws.Cells[$"{d[i]}{b[i] + 1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a[i] + 1}");
                        ExcelAroundBorder.AroundBorder(ws, $"{d[i]}{b[i] + 1}");
                    }
                }
                model = model.OrderBy(u=>u.Account).ToList();
                foreach (var item in model)
                {
                    if (col != 0)
                    {
                        if (model[col].Account.Substring(0,5) != model[col - 1].Account.Substring(0, 5))
                        {
                            ExcelAroundBorder.Book171Helper(ws, column, book171model,s2,count);
                            book171model = new Book171Excel();
                            column = a1;
                            count = 0;
                            a1++;
                        }
                    }

                    ws.Cells["H8"].Value = "Сана:";
                    ws.Cells["H8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[$"I8"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells[$"I8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"B{a1}"].Value = $"{UnicodeTo.unicodeToString(item.Account)}";
                    ws.Cells[$"B{a1}"].Style.WrapText = true;
                    ws.Cells[$"C{a1}"].Value = $"{UnicodeTo.unicodeToString(item.Name)}";
                    ws.Cells[$"C{a1}"].Style.WrapText = true;
                    ws.Cells[$"D{a1}"].Value = $"{item.SaldoBeginSumma:N}";
                    ws.Cells[$"E{a1}"].Value = $"{item.KirimSoni}";
                    ws.Cells[$"F{a1}"].Value = $"{item.KirimSumma:N}";
                    ws.Cells[$"G{a1}"].Value = $"{item.ChiqimSoni}";
                    ws.Cells[$"H{a1}"].Value = $"{item.ChiqimSumma:N}";
                    ws.Cells[$"I{a1}"].Value = $"{item.SaldoEndSumma:N}";

                    for (int i = 0; i < s1.Length; i++)
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s1[i]}{a1}");
                        ws.Cells[$"{s1[i]}{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    if (book171model.SaldoBeginSumma == null) 
                    {                        
                        book171model.SaldoBeginSumma = 0;
                        book171model.KirimSoni = 0;
                        book171model.KirimSumma = 0;
                        book171model.ChiqimSoni = 0;
                        book171model.ChiqimSumma = 0;
                        book171model.SaldoEndSumma = 0;
                    }
                    book171model.Account = item.Account;
                    book171model.SaldoBeginSumma += item.SaldoBeginSumma;
                    book171model.KirimSoni += item.KirimSoni;
                    book171model.KirimSumma += item.KirimSumma;
                    book171model.ChiqimSoni += item.ChiqimSoni;
                    book171model.ChiqimSumma += item.ChiqimSumma;
                    book171model.SaldoEndSumma += item.SaldoEndSumma;
                    count++;
                    col++;
                    a1++;

                    
                }
                ExcelAroundBorder.Book171Helper(ws, column, book171model,s2,count);

                ws.Cells[$"G{a1 + 1}"].Value = $"Кассир:";
                ws.Cells[$"H{a1 + 1}"].Value = $"{user}";
                ws.Cells[$"G{a1 + 2}"].Value = $"Бош Ҳисобчи:";
                ws.Cells[$"H{a1 + 2}"].Value = $"{model.FirstOrDefault().BoshKassir}";
                ws.Cells[$"G{a1 + 1}"].Style.Font.Bold = true;
                ws.Cells[$"G{a1 + 1}"].Style.Font.Bold = true;
                ws.Cells["B2:E2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(3).Width = 50;
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