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
    public class Book121Service : EntityRepositoryCore<Book121>, IBook121Service
    {
        private DataContext _context;
        private CommonHelper _commonHelper;

        public Book121Service(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
            _commonHelper = new CommonHelper(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(int bankCode, int userId, DateTime date)
        {

            var result = new Journal121ResultQuery();
            var dateString = date.ToString("dd.MM.yyyy");
            string sqltxt = $"select * from table(GET_BOOK_121(to_date('{dateString}','DD.MM.YYYY'),{bankCode}))";
            var entity = _context.Query<Book121QueryModel>().FromSql(sqltxt).OrderBy(f => f.Id).ToList();
            result.Data = entity;
            result.Total = entity.Count();

            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseCoreData GetCurrencyTypes()
        {
            var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
            return new ResponseCoreData(currencyTypes, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="date"></param>
        /// <param name="currencyCode"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        public ResponseCoreData SetFirstSaldo(int bankCode, string permissions, DateTime date, int currencyCode, int saldoBeginCount, double saldoBeginSumma)
        {
            if (!permissions.Contains(Permission.CashBookProcedureRun))
                return new ResponseCoreData("Нет доступ", ResponseStatusCode.BadRequest);

            var insertedRowsCount = SetSaldoBeginProcedure(bankCode, date, currencyCode, saldoBeginCount, saldoBeginSumma);
            return new ResponseCoreData(insertedRowsCount, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="currencyCode"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        private int SetSaldoBeginProcedure(int bankCode, DateTime date, int currencyCode, int saldoBeginCount, double saldoBeginSumma)
        {
            int insertedRowsCount;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SET_FIRST_SALDO_BOOK121";

                var dateParam = new OracleParameter("SANA", OracleDbType.Date, ParameterDirection.Input);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);

                var bankCodeParam = new OracleParameter("BANKKOD", OracleDbType.Int32, ParameterDirection.Input);
                bankCodeParam.Value = bankCode;
                cmd.Parameters.Add(bankCodeParam);

                var currencyCodeParam = new OracleParameter("KODVALUT", OracleDbType.Int32, ParameterDirection.Input);
                currencyCodeParam.Value = currencyCode;
                cmd.Parameters.Add(currencyCodeParam);

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
        public byte[] ToExport(List<Book121Excel> model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                double allsumma = 0;

                var ws = excel.Workbook.Worksheets.Add("Book121");
                int[] a = { 9, 9, 9, 9, 9 };
                int[] b = { 10, 10, 9, 9, 10 };
                string[] s = { "B", "C", "D", "F", "H" };
                string[] d = { "B", "C", "E", "G", "H" };
                string[] c = { "Валюта номи", "Кун бошига сальдо", "Кирим", "Чиқим", "Кун охирига сальдо" };
                int a1 = 11;
                string[] headerName = { bankName, "НАҚД ЧЕТ ЭЛ ВАЛЮТАСИ ВА ЧЕТ ЭЛ ВАЛЮТАСИДАГИ ТЎЛОВ ҲУЖЖАТЛАРИ ҚОЛДИҒИНИ ҚАЙД ЭТИШ КИТОБИ", "КИТОБИ" };
                int[] headerCount = { 2, 4, 6 };
                string[] s1 = { "B", "C", "D", "E", "F", "G", "H" };
                ws.Cells["H1"].Value = "121-Шакл";
                ws.Cells["H1"].Style.Font.Bold = true;
                ws.Cells["H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                for (int i = 0; i < headerName.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < s.Length; i++)
                {
                    ws.Column(i + 2).Width = 20;
                    ws.Column(i + 4).Width = 20;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Value = $"{c[i]}";
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Merge = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a[i]}", $"{d[i]}{b[i]}");
                    if (s[i] == "D" || s[i] == "F")
                    {
                        ws.Cells[$"{s[i]}{a[i] + 1}"].Value = "Ҳужжатлар сони";
                        ws.Cells[$"{d[i]}{b[i] + 1}"].Value = "Сумма ";
                        ws.Cells[$"{s[i]}{a[i] + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[$"{d[i]}{b[i] + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[$"{s[i]}{a[i] + 1}"].Style.Font.Bold = true;
                        ws.Cells[$"{d[i]}{b[i] + 1}"].Style.Font.Bold = true;
                        ExcelAroundBorder.AroundBorder(ws, $"{d[i]}{b[i] + 1}");
                    }
                }

                ws.Cells["G8"].Value = "Сана:";
                ws.Cells["D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"H8"].Value = $"{model.FirstOrDefault().Date:dd.MM.yyyy}";
                ws.Cells[$"h8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int j = 0; j < model.Count; j++)
                {
                    for (int i = 0; i < s1.Length; i++)
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s1[i]}{a1}");
                        ws.Cells[$"{s1[i]}{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        ws.Cells[$"{s1[i]}{a1}"].Value = GetValueFromModel(model, j, s1[i]);
                    }
                    a1++;
                }
                ws.Cells[$"G{a1 + 1}"].Value = $"Кассир:";
                ws.Cells[$"H{a1 + 1}"].Value = $"{user}";
                ws.Cells[$"G{a1 + 2}"].Value = $"Бош Ҳисобчи:";
                ws.Cells[$"H{a1 + 2}"].Value = $"{model.FirstOrDefault().BoshKassir}";
                ws.Cells[$"G{a1 + 1}"].Style.Font.Bold = true;
                ws.Cells[$"G{a1 + 1}"].Style.Font.Bold = true;
                ws.Cells["B2:E2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Column(2).Width = 30;
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetValueFromModel(List<Book121Excel> model, int j, string s1Name)
        {
            var value = string.Empty;
            switch (s1Name)
            {
                case "B":
                    value = model[j].SymbolName;
                    break;
                case "C":
                    value = $"{model[j].SaldoBeginSumma:N}";
                    break;
                case "D":
                    value = $"{model[j].KirimSoni}";
                    break;
                case "E":
                    value = $"{model[j].KirimSumma:N}";
                    break;
                case "F":
                    value = $"{model[j].ChiqimSoni}";
                    break;
                case "G":
                    value = $"{model[j].ChiqimSumma:N}";
                    break;
                case "H":
                    value = $"{model[j].SaldoEndSumma:N}";
                    break;
            }

            return value;
        }
    }
}
