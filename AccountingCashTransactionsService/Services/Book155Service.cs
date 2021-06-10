using AccountingCashTransactionsService.Interfaces;
using AutoMapper;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Microsoft.EntityFrameworkCore;
using Entitys.Models.CashOperation;
using Entitys.PostModels.CashOperations;
using System;
using System.Linq;
using Entitys.Models.Auth;
using Entitys.ViewModels.CashOperation.Book155;
using System.Collections.Generic;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.IO;
using Entitys.Helper.ExcelAround;
using System.Data;
using Entitys.ViewModels.CashOperation;
using AccountingCashTransactionsService.Helper;
using Entitys.Enums;
using Entitys.Helper.Unicode;
using Entitys.Models;

namespace AccountingCashTransactionsService.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class Book155Service : EntityRepositoryCore<Book155>, IBook155Service
    {
        private readonly DataContext _context;
        private IMapper _mapper;
        private CommonHelper _commonHelper;


        public Book155Service(IDbContext context, DataContext dBcontext, IMapper mapper) : base(context)
        {
            _context = dBcontext;
            _mapper = mapper;
            _commonHelper = new CommonHelper(dBcontext);
        }

        private Book155 toEntity(Book155Model model, int userId, DateTime date)
        {
            var result = new Book155();
            result.Date = date;
            result.From175 = false;

            if (model.CashierId == 0)
            {
                var otherInouts = _context.OutherInouts.Where(o => o.Id == model.OtherInoutId).FirstOrDefault();
                if (otherInouts == null)
                    return result;

                var _inoutId = otherInouts.InOutId;
                if (_inoutId == 1)
                {
                    result.ToCashierId = userId;
                    result.FromCashierId = 0;
                    result.Accept = true;
                }
                else
                {
                    result.ToCashierId = 0;
                    result.FromCashierId = userId;
                    result.Accept = true;
                }
            }
            else
            {
                result.ToCashierId = model.CashierId;
                result.FromCashierId = userId;
            }
            result.CashValue = model.CashValue;
            result.OperationId = model.OperationId;
            result.SprObjectId = model.SprObjectId;
            result.WorthAccount = model.WorthAccount;
            result.CounterCashierId = model.CounterCashierId;
            result.From175 = model.From175;
            result.OtherInoutId = model.OtherInoutId;

            return result;
        }

        private Book155ViewModel ToModel(int orgId, Book155 entity)
        {
            var result = new Book155ViewModel();
            result.Id = entity.Id;
            result.Date = entity.Date;
            result.FromCashierId = entity.FromCashierId;
            result.FromCashierName = GetUserFullName(entity.FromCashierId);
            result.ToCashierId = entity.ToCashierId;
            result.ToCashierName = GetUserFullName(entity.ToCashierId);
            result.CashValue = entity.CashValue;
            result.Accept = entity.Accept;
            result.OperationId = entity.OperationId;
            result.SprObjectId = entity.SprObjectId;
            result.SprObjectName = GetCurrencyName(entity.SprObjectId);
            result.WorthAccount = entity.WorthAccount;
            result.WorthAccountName = GetWorthName(orgId, entity.WorthAccount);
            result.CounterCashierId = entity.CounterCashierId;
            result.From175 = entity.From175;
            result.OtherInoutId = entity.OtherInoutId;

            return result;
        }

        public ResponseCoreData Add(int bankCode, Book155Model model, int userId, int cashierTypeId)
        {
            var date = GetWorkingDate(bankCode);
            if (isDayClosed(bankCode, date))
                return new ResponseCoreData($"Day closed for {date} date", ResponseStatusCode.BadRequest);

            var entity = toEntity(model, userId, date);
            if (entity.ToCashierId != userId)
            {
                var sum1 = _context.Book155s.Where(w => w.FromCashierId == userId &&
                w.OperationId == entity.OperationId && w.Date == entity.Date && w.SprObjectId == model.SprObjectId).
                    Sum(s => s.CashValue);

                var sum2 = _context.Book155s.Where(w =>w.ToCashierId == userId &&
                w.OperationId == entity.OperationId && w.Date == entity.Date && w.SprObjectId == model.SprObjectId).
                    Sum(s => s.CashValue);

                var sum = sum2 - sum1;
                if (sum < entity.CashValue)
                    return new ResponseCoreData("Resurs yetmasligi sababli saqlash amalga oshirilmadi!", ResponseStatusCode.BadRequest);
            }
            entity.SystemDate = DateTime.Now;
            Add(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book155, EventType.Edit);

            return new ResponseCoreData(entity, ResponseStatusCode.OK);
        }

        public ResponseCoreData DeleteById(int bankCode, int userId, int Id)
        {
            var entity = _context.Book155s.Where(f => f.Id == Id).ToList().FirstOrDefault();
            if (isDayClosed(bankCode, entity.Date))
                return new ResponseCoreData($"Day closed for {entity.Date} date", ResponseStatusCode.BadRequest);

            if (entity == null)
                return new ResponseCoreData("Not found");

            Delete(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book155, EventType.Delete);

            return new ResponseCoreData(ResponseStatusCode.OK);
        }

        public ResponseCoreData GetAll(int orgId, DateTime date, int userId, int? operationId,
            int? filterUserId, int? sprObjectId, string worthAccount)
        {
            var dateString = date.ToString("dd.MM.yyyy").Replace(".", "/");
            var operationIdSt = operationId == null ? "null" : operationId.ToString();
            var filterUserIdSt = filterUserId == null ? "null" : filterUserId.ToString();
            var sprObjectIdSt = sprObjectId == null ? "null" : sprObjectId.ToString();
            var worthAccountSt = string.IsNullOrEmpty(worthAccount) ? "''" : worthAccount;

            var script = $"select * from table(GET_BOOK155_ALL(to_date('{dateString}','dd/mm/yyyy'),{userId},{operationIdSt}," +
                $"{filterUserIdSt},{sprObjectIdSt},{worthAccountSt}))";
            var book155GetAll = _context.Query<GetBook155All>().FromSql(script).ToList();
            var result = book155GetAll.Select(ConvertToGetBook155AllViewModel).ToList();

            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }
        public ResponseCoreData GetBook155OtherInouts(int cashierTypeId)
        {
            var otherInouts = new List<Book155OtherInout>();
            if (cashierTypeId == 1)
                otherInouts = _context.OutherInouts.Where(w => w.IsMainCashier == true).Select(ConvertToOtherInoutModel).ToList();
            else
                otherInouts = _context.OutherInouts.Where(w => w.IsMainCashier == false).Select(ConvertToOtherInoutModel).ToList();

            return new ResponseCoreData(otherInouts, ResponseStatusCode.OK);
        }

        private Book155OtherInout ConvertToOtherInoutModel(OtherInout arg)
        {
            var result = new Book155OtherInout();

            result.Id = arg.Id;
            result.Name = arg.Name;
            result.InOutId = arg.InOutId;

            return result;
        }

        private GetBook155AllViewModel ConvertToGetBook155AllViewModel(GetBook155All data)
        {
            var result = new GetBook155AllViewModel();
            result.Id = data.ID;
            result.Date = data.SANA;
            result.FromCashierId = data.FROM_CASHIER_ID;
            result.ToCashierId = data.TO_CASHIER_ID;
            result.CashValue = data.CASH_VALUE;
            result.Accept = data.ACCEPT;
            result.OperationId = data.OPERATION_ID;
            result.SprObjectId = data.SPR_OBJECT_ID;
            result.WorhAccount = data.WORTH_ACCOUNT;
            result.SystemDate = data.SYSTEM_DATE;
            result.FromCashierName = data.FROM_CASHIER_NAME;
            result.ToCashierName = data.TO_CASHIER_NAME;
            result.CurrencyName = data.VALUT_NAME;
            result.WorthAccountName = data.WORTH_ACCOUNT_NAME;
            result.CounterCashierName = data.COUNTER_CASHIER_NAME;
            result.From175 = Convert.ToBoolean(data.FROM_175);

            return result;
        }


        private string GetUserFullName(int cashierId)
        {
            var user = _context.AuthUsers.Where(f => f.Id == cashierId).ToList().FirstOrDefault();
            if (user == null)
                return string.Empty;

            return $"{user.FirstName} {user.LastName}";
        }

        public ResponseCoreData GetById(int Id)
        {
            var result = _context.Book155s.Where(f => f.Id == Id).ToList().FirstOrDefault();
            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        public ResponseCoreData Update(int bankCode, Book155PutViewModel model, int userId, int cashierTypeId)
        {
            var date = GetWorkingDate(bankCode);
            if (isDayClosed(bankCode, date))
                return new ResponseCoreData($"Day closed for {date} date", ResponseStatusCode.BadRequest);

            var entitys = _context.Book155s.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
            if (entitys == null)
                return new ResponseCoreData(ResponseStatusCode.NotFound);
            var entity = _mapper.Map(model, entitys);
            entity.Date = date;
            entity.From175 = false;
            if (model.CashierId == 0)
            {
                entity.ToCashierId = userId;
                entity.FromCashierId = 0;
                entity.Accept = true;
            }
            else
            {
                entity.ToCashierId = model.CashierId;
                entity.FromCashierId = userId;
            }
            if (entity.ToCashierId != userId)
            {
                var sum = _context.Book155s.Where(w => w.FromCashierId != userId && w.ToCashierId == userId && w.OperationId == entity.OperationId && w.Date == entity.Date
                && w.Id != entity.Id && w.From175 == false).Sum(s => s.CashValue);


                if (sum < entity.CashValue)
                    return new ResponseCoreData("Resurs yetmasligi sababli saqlash amalga oshirilmadi!", ResponseStatusCode.BadRequest);
            }
            Update(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Book155, EventType.Edit);

            return new ResponseCoreData(ResponseStatusCode.OK);
        }

        public ResponseCoreData SubmitAcceptance(AcceptanceViewModel model)
        {
            var entity = _context.Book155s.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
            if (entity == null)
                return new ResponseCoreData(ResponseStatusCode.NotFound);
            entity.Accept = model.IsAccept;
            Update(entity);

            return new ResponseCoreData(ResponseStatusCode.OK);
        }

        public ResponseCoreData GetCashiers(int orgId)
        {
            var users = _context.AuthUsers.Where(w => w.OrgId == orgId && !w.IsAdmin).ToList();
            var result = users.Select(ConvertToCashierViewModel);
            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }
        public ResponseCoreData GetReCounterCashiers(int orgId)
        {
            var users = _context.AuthUsers.Where(w => w.OrgId == orgId && !w.IsAdmin && w.CashierTypeId == 5).ToList();
            var result = users.Select(ConvertToCashierViewModel);
            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        public ResponseCoreData GetCashiersWithType(int orgId)
        {
            var users = _context.AuthUsers.Where(w => w.OrgId == orgId && !w.IsAdmin).ToList();
            var result = users.Select(ConvertToCashierViewModels);
            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        private CashierViewModel ConvertToCashierViewModel(AuthUsers user)
        {
            var result = new CashierViewModel();
            result.Id = user.Id;
            result.FullName = $"{user.FirstName} {user.LastName}";
            result.CashierTypeId = user.CashierTypeId;
            return result;
        }
        private CashierViewModel ConvertToCashierViewModels(AuthUsers user)
        {
            var cashierTypes = _context.CashierTypes.Where(f => f.Id == user.CashierTypeId).ToList();
            string cashierTypesName = cashierTypes.FirstOrDefault().Name = cashierTypes.FirstOrDefault().Name != null ? cashierTypes.FirstOrDefault().Name : string.Empty;
            var result = new CashierViewModel();
            result.Id = user.Id;
            result.FullName = $"{user.FirstName} {user.LastName} ({UnicodeTo.unicodeToString(cashierTypesName)})";
            result.CashierTypeId = user.CashierTypeId;
            return result;
        }

        public ResponseCoreData GetCurrencyTypes()
        {
            var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
            return new ResponseCoreData(currencyTypes, ResponseStatusCode.OK);
        }

        public ResponseCoreData GetWorthAccounts(int orgId)
        {
            var worths = _context.Query<GetWorthAll>().FromSql($"select * from table(GET_WORTH_ALL({orgId}))").ToList();
            return new ResponseCoreData(worths, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData IsDayClosed(int bankCode, DateTime date)
        {
            var result = new DayClosedViewModel();
            result.IsDayClosed = isDayClosed(bankCode, date);
            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        private bool isDayClosed(int bankCode, DateTime date)
        {
            var dateString = date.ToString("dd.MM.yyyy");

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = cmd.Connection.State == ConnectionState.Open;
                if (!wasOpen) cmd.Connection.Open();
                try
                {
                    cmd.CommandText = $"select IS_CLOSE_BOOK155(to_date('{dateString}', 'DD.MM.YYYY'), {bankCode}) as result from dual";
                    var scalaerResult = cmd.ExecuteScalar();
                    var isDayClosed = scalaerResult == DBNull.Value ? false : Convert.ToBoolean((decimal)scalaerResult);
                    return isDayClosed;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (!wasOpen) cmd.Connection.Close();
                }
            }
        }

        private string GetCurrencyName(int? currencyCode)
        {
            if (currencyCode == null)
                return string.Empty;

            var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
            var currencyName = currencyTypes.FirstOrDefault(f => f.Kod == currencyCode);
            return currencyName?.Name;
        }

        private string GetWorthName(int orgId, string worthAccount)
        {
            if (string.IsNullOrEmpty(worthAccount))
                return string.Empty;

            var worths = _context.Query<GetWorthAll>().FromSql($"select * from table(GET_WORTH_ALL({orgId}))").ToList();
            var worthName = worths.FirstOrDefault(f => f.Account == worthAccount);
            return worthName?.Name;
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
        public byte[] ToExport(List<Book155ViewModels> model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            int operId = model.FirstOrDefault(f => f.OperationId != 0).OperationId;
            Console.WriteLine(operId);
            if (operId == 1)
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                double summa1 = 0;
                double summa2 = 0;
                var ws = excel.Workbook.Worksheets.Add("Book155");
                ws.Cells["D1"].Value = "155-Шакл";
                ws.Cells["D1"].Style.Font.Bold = true;
                ws.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B2:E2"].Value = bankName;
                ws.Cells["B2:E2"].Merge = true;
                ws.Cells["B2:E2"].Style.Font.Bold = true;
                ws.Cells["B2:E2"].Style.WrapText = true;
                ws.Cells["B2:E2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:E4"].Value = "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА ";
                ws.Cells["B4:E4"].Merge = true;
                ws.Cells["B4:E4"].Style.Font.Bold = true;
                ws.Cells["B4:E4"].Style.WrapText = true;
                ws.Cells["B4:E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:E6"].Value = "КИТОБИ ";
                ws.Cells["B6:E6"].Merge = true;
                ws.Cells["B6:E6"].Style.Font.Bold = true;
                ws.Cells["B6:E6"].Style.WrapText = true;
                ws.Cells["B6:E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                int a1 = 11;
                int[] a = { 9, 9, 9 };
                int[] b = { 10, 10, 9 };
                string[] s = { "B", "C", "D" };
                string[] s1 = { "B", "C", "D", "E" };
                string[] d = { "B", "C", "E" };
                string[] c = { "Вакт", "ФИШ", "Сумма" };
                for (int i = 0; i < s.Length; i++)
                {
                    ws.Column(i + 2).Width = 30;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Value = $"{c[i]}";
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Merge = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a[i]}", $"{d[i]}{b[i]}");
                }
                ws.Column(5).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(2).Width = 15;
                ws.Cells["D10"].Value = "Кирим";
                ws.Cells["D10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["D10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["E10"].Value = "Чиқим";
                ws.Cells["E10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["E10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["E10"].Style.Font.Bold = true;
                ws.Cells["D10"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"D10");
                ExcelAroundBorder.AroundBorder(ws, $"E10");
                foreach (var item in model)
                {
                    for (int i = 0; i < s1.Length; i++)
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s1[i]}{a1}");
                        ws.Cells[$"{s1[i]}{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    ws.Cells["D8"].Value = "Сана";
                    ws.Cells[$"E8"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells[$"B{a1}"].Value = $"{item.Sana:HH:mm}";
                    ws.Cells[$"C{a1}"].Value = $"{item.ToCashierName}";
                    ws.Cells[$"C{a1}"].Style.WrapText = true;
                    if (item.FromCashierId != item.UserId)
                    {
                        ws.Cells[$"D{a1}"].Value = $"{item.CashValue:N}";
                        ws.Cells[$"E{a1}"].Value = $"{0:N}";
                        summa1 += item.CashValue;
                    }
                    else
                    {
                        ws.Cells[$"D{a1}"].Value = $"{0:N}";
                        ws.Cells[$"E{a1}"].Value = $"{item.CashValue:N}";
                        summa2 += item.CashValue;
                    }
                    a1++;
                }
                ws.Cells[$"B{a1}:C{a1}"].Value = "Жами:";
                ws.Cells[$"B{a1}:C{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"B{a1}:C{a1}"].Style.Font.Bold = true;
                ws.Cells[$"B{a1}:C{a1}"].Merge = true;
                ws.Cells[$"F{a1}"].Style.Font.Bold = true;
                ws.Cells[$"F{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"E{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"E{a1}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"B{a1}", $"C{a1}");
                ExcelAroundBorder.AroundBorder(ws, $"D{a1}");
                ExcelAroundBorder.AroundBorder(ws, $"E{a1}");
                ws.Cells[$"D{a1}"].Value = $"{summa1:N}";
                ws.Cells[$"E{a1}"].Value = $"{summa2:N}";
                ws.Cells[$"D{a1}"].Style.Font.Bold = true;
                ws.Cells[$"E{a1}"].Style.Font.Bold = true;
                ws.Cells[$"D{a1 + 1}"].Value = $"Кассир:";
                ws.Cells[$"E{a1 + 1}"].Value = $"{user}";
                ws.Cells[$"D{a1 + 2}"].Value = $"Бош Ҳисобчи:";
                ws.Cells[$"E{a1 + 2}"].Value = $"{model.FirstOrDefault().BoshKassir}";
                ws.Cells[$"D{a1 + 2}"].Style.Font.Bold = true;
                ws.Cells[$"D{a1 + 1}"].Style.Font.Bold = true;
                ws.Cells["B2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }
            else
                return ToExport12(model, user);
        }
        private byte[] ToExport12(List<Book155ViewModels> model, string user)
        {
            try
            {
                var operId = model.FirstOrDefault(f => f.OperationId != 0).OperationId;
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                double summa1 = 0;
                double summa2 = 0;
                var ws = excel.Workbook.Worksheets.Add("Book155");
                ws.Cells["B2:F2"].Value = "ТОШКЕНТ Ш., \"ИПОТЕКА - БАНК\" АТИБ МЕХНАТ ФИЛИАЛИ";
                ws.Cells["B2:F2"].Merge = true;
                ws.Cells["B2:F2"].Style.Font.Bold = true;
                ws.Cells["B2:F2"].Style.WrapText = true;
                ws.Cells["B2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:F4"].Value = "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА ";
                ws.Cells["B4:F4"].Merge = true;
                ws.Cells["B4:F4"].Style.Font.Bold = true;
                ws.Cells["B4:F4"].Style.WrapText = true;
                ws.Cells["B4:F4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:F6"].Value = "КИТОБИ ";
                ws.Cells["B6:F6"].Merge = true;
                ws.Cells["B6:F6"].Style.Font.Bold = true;
                ws.Cells["B6:F6"].Style.WrapText = true;
                ws.Cells["B6:F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                int a1 = 11;
                int[] a = { 9, 9, 9, 9 };
                int[] b = { 10, 10, 10, 9 };
                string[] s = { "B", "C", "D", "E" };
                string[] d = { "B", "C", "D", "F" };
                string[] s1 = { "B", "C", "D", "E", "F" };
                string[] c = { "Вакт", "ФИШ", "Халқаро валюта", "Сумма" };
                for (int i = 0; i < s.Length; i++)
                {
                    ws.Column(i + 2).Width = 30;
                    if (c[i] == "Халқаро валюта" && operId == 3)
                        ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Value = $"Қимматлик";
                    else
                        ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Value = $"{c[i]}";
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Merge = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{s[i]}{a[i]}:{d[i]}{b[i]}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a[i]}", $"{d[i]}{b[i]}");
                }
                ws.Column(6).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(2).Width = 15;
                ws.Cells["E10"].Value = "Кирим";
                ws.Cells["E10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["E10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["F10"].Value = "Чиқим";
                ws.Cells["F10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["F10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["F10"].Style.Font.Bold = true;
                ws.Cells["E10"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"E10");
                ExcelAroundBorder.AroundBorder(ws, $"F10");
                foreach (var item in model)
                {
                    for (int i = 0; i < s1.Length; i++)
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s1[i]}{a1}");
                        ws.Cells[$"{s1[i]}{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    ws.Cells["D8"].Value = "Сана";
                    ws.Cells[$"E8"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells[$"B{a1}"].Value = $"{item.Sana:HH:mm}";
                    ws.Cells[$"C{a1}"].Value = $"{item.ToCashierName}";
                    ws.Cells[$"C{a1}"].Style.WrapText = true;
                    if (operId == 3)
                        ws.Cells[$"D{a1}"].Value = $"{item.WorthAccountName}";
                    else
                        ws.Cells[$"D{a1}"].Value = $"{item.CurrencyName}";

                    if (item.FromCashierId != item.UserId)
                    {
                        ws.Cells[$"E{a1}"].Value = $"{item.CashValue:N}";
                        ws.Cells[$"F{a1}"].Value = $"{0:N}";
                        summa1 += item.CashValue;
                    }
                    else
                    {
                        ws.Cells[$"E{a1}"].Value = $"{0:N}";
                        ws.Cells[$"F{a1}"].Value = $"{item.CashValue:N}";
                        summa2 += item.CashValue;
                    }
                    a1++;
                }
                ws.Cells[$"B{a1}:D{a1}"].Value = "Жами:";
                ws.Cells[$"B{a1}:D{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"B{a1}:D{a1}"].Style.Font.Bold = true;
                ws.Cells[$"B{a1}:D{a1}"].Merge = true;
                ws.Cells[$"D{a1}"].Style.Font.Bold = true;
                ws.Cells[$"D{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"E{a1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"D{a1}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"B{a1}", $"D{a1}");
                ExcelAroundBorder.AroundBorder(ws, $"E{a1}");
                ExcelAroundBorder.AroundBorder(ws, $"F{a1}");
                ws.Cells[$"E{a1}"].Value = $"{summa1:N}";
                ws.Cells[$"F{a1}"].Value = $"{summa2:N}";
                ws.Cells[$"E{a1}"].Style.Font.Bold = true;
                ws.Cells[$"F{a1}"].Style.Font.Bold = true;
                ws.Cells[$"E{a1 + 1}"].Value = $"Кассир:";
                ws.Cells[$"F{a1 + 1}"].Value = $"{user}";
                ws.Cells[$"E{a1 + 2}"].Value = $"Бош Ҳисобчи:";
                ws.Cells[$"F{a1 + 2}"].Value = $"{model.FirstOrDefault().BoshKassir}";
                ws.Cells[$"E{a1 + 2}"].Style.Font.Bold = true;
                ws.Cells[$"E{a1 + 1}"].Style.Font.Bold = true;
                ws.Cells["B2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:F4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime GetWorkingDate(int bankCode)
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
    }
}
