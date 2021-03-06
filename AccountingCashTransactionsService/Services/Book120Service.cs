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
using Entitys.Helper.Translate;
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
    public class Book120Service : EntityRepositoryCore<Book120>, IBook120Service
    {
        private DataContext _context;
        private CommonHelper _commonHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexts"></param>
        /// <param name="context"></param>
        public Book120Service(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
            _commonHelper = new CommonHelper(context);
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

            var result = new Journal109ResultQuery();
            var dates = date.ToString("dd.MM.yyyy");
            string sqltxt = $"select * from table(GET_BOOK_120(to_date('{dates}','DD.MM.YYYY'),{bankCode}))";
            var entity = _context.Query<QueryModel>().FromSql(sqltxt).OrderBy(f => f.Id).ToList();
            result.Data = entity;
            result.Total = entity.Count();

            return new ResponseCoreData(result, ResponseStatusCode.OK);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="date"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        public ResponseCoreData SetFirstSaldo(int bankCode, string permissions, DateTime date, int saldoBeginCount, double saldoBeginSumma)
        {
            if (!permissions.Contains(Permission.CashBookProcedureRun))
                return new ResponseCoreData("Нет доступ", ResponseStatusCode.BadRequest);


            var insertedRowsCount = SetSaldoBeginProcedure(bankCode, date, saldoBeginCount, saldoBeginSumma);
            return new ResponseCoreData(insertedRowsCount, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="saldoBeginCount"></param>
        /// <param name="saldoBeginSumma"></param>
        /// <returns></returns>
        private int SetSaldoBeginProcedure(int bankCode, DateTime date, int saldoBeginCount, double saldoBeginSumma)
        {
            int insertedRowsCount;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SET_FIRST_SALDO_BOOK120";

                var dateParam = new OracleParameter("SANA", OracleDbType.Date, ParameterDirection.Input);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);

                var bankCodeParam = new OracleParameter("BANKKOD", OracleDbType.Int32, ParameterDirection.Input);
                bankCodeParam.Value = bankCode;
                cmd.Parameters.Add(bankCodeParam);

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
        public ResponseCoreData GetBankNameById(int bankId)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public byte[] ToExport(List<ExcelModelForBook141> model, string user, int bankCode)
        {
            var bankName = GetBankName(bankCode);
            try
            {
                string[] s = { "B", "C", "D" };
                string[] columName = { "Айланма кассадаги нақд пуллар сўмлар:", "Сони", "Сумма" };
                int[] columWidth = { 30, 20, 25 };
                var a = 10;
                string[] headerName = { bankName, "АЙЛАНМА КАССАДАГИ НАҚД МИЛЛИЙ ВАЛЮТАНИНГ ҲИСОБИНИ ЮРИТИШ КИТОБИ", "КИТОБИ" };
                double summa = 0;
                int count = 0;
                int[] headerCount = { 2, 4, 6 };
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Book120");
                ws.Cells["D1"].Value = "120-Шакл";
                ws.Cells["D1"].Style.Font.Bold = true;
                ws.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                for (int i = 0; i < headerName.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < columWidth.Length; i++)
                {
                    ws.Column(i + 2).Width = columWidth[i];
                    ws.Column(i + 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    if (s[i] == "B")
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}8", $"{s[i]}9");
                        ws.Cells[$"{s[i]}8:{s[i]}9"].Value = $"{columName[i]}";
                        ws.Cells[$"{s[i]}8:{s[i]}9"].Merge = true;
                        ws.Cells[$"{s[i]}8:{s[i]}9"].Style.WrapText = true;
                        ws.Cells[$"{s[i]}8:{s[i]}9"].Style.Font.Bold = true;
                        ws.Cells[$"{s[i]}8:{s[i]}9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    else
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}9");
                        ws.Cells[$"{s[i]}9"].Value = $"{columName[i]}";
                        ws.Cells[$"{s[i]}9"].Style.Font.Bold = true;
                    }

                }
                foreach (var item in model)
                {
                    for (int i = 0; i < s.Length; i++)
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");
                    ws.Cells["C8:D8"].Value = $"Сана: {item.Date:dd.MM.yyyy}";
                    ws.Cells["C8:D8"].Style.Font.Bold = true;
                    ws.Cells["C8:D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ExcelAroundBorder.AroundBorder(ws, $"C8", "D8");
                    ws.Cells["C8:D8"].Merge = true;
                    ws.Cells[$"B{a}"].Value = $"{TranslateBooks.GetTranslateBook(item.Name)}";
                    ws.Cells[$"C{a}"].Value = $"{item.Count}";
                    ws.Cells[$"D{a}"].Value = $"{item.Summa:N}";
                    summa = summa + item.Summa;
                    count += item.Count;
                    a++;
                }

                ws.Cells[$"C{a + 1}"].Value = $"Кассир:";
                ws.Cells[$"D{a + 1}"].Value = $"{user}";
                ws.Cells[$"C{a + 2}"].Value = $"Бош Ҳисобчи:";
                ws.Cells[$"D{a + 2}"].Value = $"{model.FirstOrDefault().BoshKassir}";
                ws.Cells[$"C{a + 2}"].Style.Font.Bold = true;
                ws.Cells[$"C{a + 1}"].Style.Font.Bold = true;
                ws.Cells["B2:E2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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