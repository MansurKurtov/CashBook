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
using Entitys.ViewModels.CashOperation.Collector;
using Entitys.ViewModels.CashOperation.Journal15;
using Entitys.ViewModels.CashOperation.Journal16VM;
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
    public class Journal16Service : EntityRepositoryCore<Journal16>, IJournal16Service
    {
        private DataContext _context;
        private IMapper _mapper;
        private CommonHelper _commonHelper;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexts"></param>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        public Journal16Service(IDbContext contexts, IMapper mapper, DataContext context) : base(contexts)
        {
            _context = context;
            _mapper = mapper;
            _commonHelper = new CommonHelper(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="cashierTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Add(int bankCode, int cashierTypeId, int userId, Journal16PostViewModel model)
        {
            int newId;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SET_JOURNAL_16_ADD";

                var directionNumber = new OracleParameter("DIRECTION_NUMBER", OracleDbType.NVarchar2, ParameterDirection.Input);
                directionNumber.Value = model.DirectionNumber;
                cmd.Parameters.Add(directionNumber);

                var sana = new OracleParameter("SANA", OracleDbType.Date, ParameterDirection.Input);
                sana.Value = model.Date;
                cmd.Parameters.Add(sana);

                var userID = new OracleParameter("USER_ID", OracleDbType.Int32, ParameterDirection.Input);
                userID.Value = userId;
                cmd.Parameters.Add(userID);

                var systemDate = new OracleParameter("SYSTEM_DATE", OracleDbType.Date, ParameterDirection.Input);
                systemDate.Value = DateTime.Now;
                cmd.Parameters.Add(systemDate);

                var acceptParam = new OracleParameter("ACCEPT", OracleDbType.Int32, ParameterDirection.Input);
                acceptParam.Value = false;
                cmd.Parameters.Add(acceptParam);

                var acceptDateParam = new OracleParameter("ACCEPT_DATE", OracleDbType.Date, ParameterDirection.Input);
                acceptDateParam.Value = null;
                cmd.Parameters.Add(acceptDateParam);

                var statusParam = new OracleParameter("STATUS", OracleDbType.Int32, ParameterDirection.Input);
                statusParam.Value = cashierTypeId == 4 ? 1 : 2;
                cmd.Parameters.Add(statusParam);

                var newIdParam = new OracleParameter("NEW_ID", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(newIdParam);

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                DataTable budt = new DataTable();
                while (reader.Read())
                {
                    budt.Load(reader);
                }
                newId = int.Parse(newIdParam.Value.ToString());
                cmd.Connection.Close();
            }

            var result = new Journal16AddReturnViewModel();
            result.Id = newId;

            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseCoreData Update(int bankCode, int userId, Journal16PutViewModel model)
        {
            var entity = _context.Journal16s.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
            if (entity == null)
                return new ResponseCoreData(new Exception("Not Found"));

            entity = _mapper.Map(model, entity);
            entity.Date = model.Date;
            //entity.Date = GetWorkingDate(bankCode);                
            entity.UserId = userId;

            _context.Journal16s.Update(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal16, EventType.Edit);

            return new ResponseCoreData(entity, ResponseStatusCode.OK);
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
            var entity = _context.Journal16s.Where(f => f.Id == Id).ToList().FirstOrDefault();
            if (entity == null)
                return new ResponseCoreData(new Exception("Not Found"));
            Delete(entity);

            _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal16, EventType.Delete);

            return new ResponseCoreData(ResponseStatusCode.OK);
        }


        public ResponseCoreData SetAcceptance(int id, bool acceptance)
        {
            var item = _context.Journal16s.Where(f => f.Id == id).ToList().FirstOrDefault();
            if (item == null)
                return new ResponseCoreData("Not found!", ResponseStatusCode.BadRequest);
            item.Accept = acceptance;
            item.AcceptDate = DateTime.Now;
            Update(item);
            return new ResponseCoreData(item, ResponseStatusCode.OK);
        }

        public ResponseCoreData GetAllWithJournal15(int orgId, int cashierTypeId, DateTime date, int? status, int skip, int take)
        {
            var result = new Journal16WithJournal15ResultViewModel();

            var userIds = _context.AuthUsers.Where(w => w.OrgId == orgId).Select(s => s.Id).ToList();

            List<Journal16> findResult;
            if (cashierTypeId == 5 && status != 0)
                findResult = Find(f => userIds.Contains(f.UserId) && f.Date == date && f.Status == status).ToList();
            else if (status != 0)
                findResult = Find(f => userIds.Contains(f.UserId) && f.Date == date && f.Status == 1).ToList();
            else
                findResult = Find(f => userIds.Contains(f.UserId) && f.Date == date).ToList();

            result.Total = findResult.Count();
            var data = findResult.Skip(skip * take).Take(take).Select(ConvertToViewModel).ToList();
            result.Data = data;

            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseCoreData GetById(int userId, int id)
        {
            var entity = _context.Journal16s.Where(f => f.Id == id).ToList().FirstOrDefault();

            return new ResponseCoreData(entity, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(int userId, DateTime date)
        {
            var entity = _context.Journal16s.Where(f => f.Date == date).ToList().FirstOrDefault();
            return new ResponseCoreData(entity, ResponseStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public ResponseCoreData GetSupervisingAccountantFullName(int bankCode)
        {
            var item = _context.SupervisingAccountants.Where(f => f.BankCode == bankCode).ToList().FirstOrDefault();
            var result = new SupervisingAccountantViewModel();
            if (item == null)
                result.FullName = string.Empty;
            else result.FullName = item.Fio;

            return new ResponseCoreData(result, ResponseStatusCode.OK);
        }

        private Journal16ViewModel ConvertToViewModel(Journal16 entity)
        {
            var result = new Journal16ViewModel();
            result.Id = entity.Id;
            result.DirectionNumber = entity.DirectionNumber;
            result.Date = entity.Date;
            result.Accept = entity.Accept;
            result.AcceptDate = entity.AcceptDate;
            result.UserId = entity.UserId;
            result.SystemDate = entity.SystemDate;
            result.Journal15List = GetJournal15List(entity.Id);
            result.Journal15EmptyBags = GetJournal15EmptyBagsList(entity.Id);
            result.BagsCount = result.Journal15List.Count;
            result.CurrencyInfos = GetJournal15CurrencyInfo(entity.Id);
            result.EmptyBags = result.Journal15EmptyBags.Count;
            result.Summa = result.Journal15List.Sum(s => s.Summa);
            result.CollectorFioList = GetCollectorFioList(entity.Id);
            result.Status = entity.Status;

            return result;
        }

        private List<Journal15CurrencyViewModel> GetJournal15CurrencyInfo(int id)
        {
            var currencyes = _context.Journal15s.Where(w => w.Journal16Id == id).GroupBy(g => g.SprObjectId).
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

        private string GetCurrencyTypeById(int id)
        {
            var currencyTypes = _context.Query<GetValType>().FromSql("select * from table(GET_VALUT_ALL)").ToList();
            var result = currencyTypes.Where(w => w.Kod == id).FirstOrDefault();
            if (result?.Name == null)
                return "Сўм";

            return result?.Name;
        }
        private List<CollectorViewModel> GetCollectorFioList(int journal16Id)
        {
            var entities = _context.Collectors.Where(w => w.Journal16Id == journal16Id).ToList();
            if (entities == null)
                return null;

            return entities.Select(ConvertToCollectorFio).ToList();
        }

        private CollectorViewModel ConvertToCollectorFio(Collector entity)
        {
            var result = new CollectorViewModel();
            result.Id = entity.Id;
            result.Journal16Id = entity.Journal16Id;
            result.Fio = entity.Fio;
            result.SystemDate = entity.SystemDate;
            return result;
        }

        private List<Journal15ViewModel> GetJournal15List(int journal16Id)
        {
            var entities = _context.Journal15s.Where(f => f.Journal16Id == journal16Id && f.IsEmpty == false).ToList();
            if (entities == null)
                return null;
            return entities.Select(ConvertToJournal15ViewModel).ToList();
        }

        private List<Journal15ViewModel> GetJournal15EmptyBagsList(int journal16Id)
        {
            var entities = _context.Journal15s.Where(f => f.Journal16Id == journal16Id && f.IsEmpty == true).ToList();
            if (entities == null)
                return null;
            return entities.Select(ConvertToJournal15ViewModel).ToList();
        }

        private Journal15ViewModel ConvertToJournal15ViewModel(Journal15 entity)
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
            result.CurrencyName = GetCurrencyTypeById(entity.SprObjectId);

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
                result.IsDayClosed = commonHelper.IsDayClosed((int)ModuleType.Journal16, bankCode, date);
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
        public byte[] ToExport(List<ExcelModelForJournal16> model, string user, int bankCode, int CashierTypeId)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                double summa = 0;
                string[] head = { bankName, "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА", "КИТОБИ" };
                string[] headcoll = { "B2", "B4", "B6" };
                string[] headrow = { "H2", "H4", "H6" };
                var ws = excel.Workbook.Worksheets.Add("Journal18Front");
                int a = 12;
                int[] columnWith = { 7, 23, 20, 20, 20, 30, 25, 20, 20 };
                for (int i = 0; i < head.Length; i++)
                {
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Value = $"{head[i]}";
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Merge = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.WrapText = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                string[] s1 = { "B", "C", "D", "E", "F", "G", "H", "F", "D" };
                int[] w1 = { 9, 9, 10, 10, 11, 11, 11, 10, 9 };

                string[] s2 = { "B", "C", "D", "E", "F", "G", "H", "H", "H" };
                int[] w2 = { 11, 11, 11, 11, 11, 11, 11, 10, 9 };

                string[] c = { "Т/р", "Йўналиш (ташриф) рақами", "Инкассатор(ФИШ)", "Қоплар сони", "Сумма", "Валюта номи", "Сумма (сўз билан)", "бўйича тушум пулларининг эълон қилинган (қайд этилган) умумий суммаси (сўмда)", "Банк (филиал) кассасига қабул қилинган инкассация сумка(қоп)лари" };
                string[] s = { "B", "C", "D", "E", "F", "G", "H" };
                string[] s3 = { "B", "C", "D", "E", "F" };
                for (int i = 0; i < s1.Length; i++)
                {


                    ws.Column(i + 2).Width = columnWith[i];
                    ws.Cells[$"{s1[i]}{w1[i]}:{s2[i]}{w2[i]}"].Value = $"{c[i]}";
                    ws.Cells[$"{s1[i]}{w1[i]}:{s2[i]}{w2[i]}"].Merge = true;
                    ws.Cells[$"{s1[i]}{w1[i]}:{s2[i]}{w2[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{s1[i]}{w1[i]}:{s2[i]}{w2[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s1[i]}{w1[i]}:{s2[i]}{w2[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{s1[i]}{w1[i]}:{s2[i]}{w2[i]}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[$"{s1[i]}{w1[i]}:{s2[i]}{w2[i]}"].Style.WrapText = true;
                    ExcelAroundBorder.AroundBorder(ws, $"{s1[i]}{w1[i]}", $"{s2[i]}{w2[i]}");
                }
                foreach (var item in model)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");
                        ws.Cells[$"{s[i]}{a}"].Style.WrapText = true;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        ws.Cells[$"{s[i]}{a}:{s[i]}{a + item.CurrencyList.Count - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[$"{s[i]}{a}:{s[i]}{a + item.CurrencyList.Count - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[$"{s[i]}{a}:{s[i]}{a + item.CurrencyList.Count - 1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}:{s[i]}{a + item.CurrencyList.Count - 1}");
                        ws.Cells[$"{s[i]}{a}:{s[i]}{a + item.CurrencyList.Count - 1}"].Style.WrapText = true;
                        ws.Cells[$"{s[i]}{a}:{s[i]}{a + item.CurrencyList.Count - 1}"].Merge = true;
                    }
                    ws.Cells[$"B{a}:B{a + item.CurrencyList.Count - 1}"].Value = $"{a - 11}";
                    ws.Cells[$"C{a}:C{a + item.CurrencyList.Count - 1}"].Value = $"{item.DirectionNumber}";
                    ws.Cells[$"D{a}:D{a + item.CurrencyList.Count - 1}"].Value = $"{item.CollectorName}";

                    foreach (var item2 in item.CurrencyList)
                    {
                        for (int i = 3; i < s.Length; i++)
                        {
                            ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");
                            ws.Cells[$"{s[i]}{a}"].Style.WrapText = true;
                        }
                        ws.Cells[$"E{a}"].Value = $"{item2.Count}";
                        ws.Cells[$"F{a}"].Value = $"{item2.Value}";
                        ws.Cells[$"G{a}"].Value = $"{item2.Name}";
                        ws.Cells[$"H{a}"].Value = $"{item2.WordName}";
                        a++;
                    }


                }
                ws.Cells[$"B{a + 1}:G{a + 1}"].Value = "сўм тушум пуллари солинганлиги эълон қилинган сумкалар (қоплар) қабул қилинди. Ушбу қабул қилинган пуллик сумкалар (қоплар)";
                ws.Cells[$"B{a + 2}:G{a + 2}"].Value = "орасидан қуйидаги нуқсонли (шикастланган) сумкалар (қоплар) банк (филиал) _________________ кассасида инкассаторлар ";
                ws.Cells[$"B{a + 3}:G{a + 3}"].Value = "иштирокида очилиб, ундаги нақд пул тушумлари қайта саналиб";
                ws.Cells[$"B{a + 1}:G{a + 1}"].Merge = true;
                ws.Cells[$"B{a + 2}:G{a + 2}"].Merge = true;
                ws.Cells[$"B{a + 3}:G{a + 3}"].Merge = true;
                a += 4;

                string[] s4 = { "B", "C", "D", "E", "F", "D" };
                string[] s5 = { "B", "C", "D", "E", "F", "F" };
                int[] w5 = { a, a, a + 1, a + 1, a + 1, a };
                int[] w6 = { a + 1, a + 1, a + 1, a + 1, a + 1, a };

                string[] c1 = { "Т/р", "Йўналиш (ташриф) рақами", "сумка(қоп) рақами", "сумма рақам билан", "сумма сўз билан", "Очилган инкассация сумка (қопи) ва уларга солинган нақд пул тушумларининг суммаси (сўмда)" };

                for (int i = 0; i < s4.Length; i++)
                {
                    ExcelAroundBorder.ExcelWidth(ws, c1[i], i + 2);
                    ws.Cells[$"{s4[i]}{w5[i]}:{s5[i]}{w6[i]}"].Value = $"{c1[i]}";
                    ws.Cells[$"{s4[i]}{w5[i]}:{s5[i]}{w6[i]}"].Merge = true;
                    ws.Cells[$"{s4[i]}{w5[i]}:{s5[i]}{w6[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{s4[i]}{w5[i]}:{s5[i]}{w6[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{s4[i]}{w5[i]}:{s5[i]}{w6[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{s4[i]}{w5[i]}:{s5[i]}{w6[i]}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[$"{s4[i]}{w5[i]}:{s5[i]}{w6[i]}"].Style.WrapText = true;
                    ExcelAroundBorder.AroundBorder(ws, $"{s4[i]}{w5[i]}", $"{s5[i]}{w6[i]}");
                }
                a += 2;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < s3.Length; j++)
                    {
                        ExcelAroundBorder.AroundBorder(ws, $"{s3[j]}{a}");
                        ws.Cells[$"{s3[j]}{a}"].Value = $"";
                    }
                    a++;
                }
                string col = model.FirstOrDefault().AcceptDate != null ? $"{model.FirstOrDefault().AcceptDate} да қайта санаш кассасининг назоратчиси кечки касса" : "_______________ да қайта санаш кассасининг назоратчиси кечки касса ";
                string[] column = { $"сўм тушум пуллари қайта санаб қабул қилинди.", $"Бўш сумка (қоп) банк (филиал) кассасига қабул қилинмади.", $"Кассир:", $"{model.FirstOrDefault().CashierName}__________", $"Бухгалтер-назоратчи:", $"{model.FirstOrDefault().SuperVisorName}__________", $"II {model.FirstOrDefault().Date}да касса мудири банк(филиал)нинг кечки кассада очилган нуқсонли сумка (қоп)дан чиққан ", $"______________________________________ сўмлик", $"(сумма рақам ва сўз билан)", $"нақд пулларни, ушбу сумка (қоп)ни ҳужжатлари ва тузилган далолатнома (17-илова)да қайд этилган маълумотлар билан таққослаб кўрган ҳолда кечки касса", $"кассиридан қабул қилиб олди. ", $"Кечки касса ходимларидан қабул қилиб олди:", $"________________________ _________ ", $"        (Ф.И.О.)         (имзо) ", col, $"ходимларидан___________________________________________та тушум пуллари", $"                                           (қопларнинг сони рақам ва сўз билан)", $"солинган ва _______________________________________________________та бўш инкассация сумкаси (қопи)ни қабул қилиб олди.", "                                           (қопларнинг сони рақам ва сўз билан)", $"Кечки касса ходимларидан қабул қилиб олди:", $"Қайта санаш кассасининг назоратчиси: {model.FirstOrDefault().CashierName}____________" };
                bool[] columBool = { false, false, true, false, true, false, false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false };
                bool[] columBold = { false, false, true, false, true, false, false, false, false, false, true, false, true, true, true, false, false, false, false, true, true, false };
                string[] columnName1 = { "B", "B", "F", "G", "E", "G", "B", "B", "B", "B", "B", "B", "E", "E", "B", "B", "B", "B", "B", "B", "E" };
                string[] columnName2 = { "G", "G", "F", "G", "F", "G", "G", "G", "G", "G", "G", "G", "G", "G", "G", "G", "G", "G", "G", "G", "G" };
                for (int i = 0; i < column.Length; i++)
                {
                    ws.Cells[$"{columnName1[i]}{a}:{columnName2[i]}{a}"].Value = column[i];
                    if (columBool[i] == true)
                        ws.Cells[$"{columnName1[i]}{a}:{columnName2[i]}{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    else
                        ws.Cells[$"{columnName1[i]}{a}:{columnName2[i]}{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[$"{columnName1[i]}{a}:{columnName2[i]}{a}"].Merge = true;
                    ws.Cells[$"{columnName1[i]}{a}:{columnName2[i]}{a}"].Style.Font.Bold = columBold[i];
                    ws.Cells[$"{columnName1[i]}{a}:{columnName2[i]}{a}"].Style.WrapText = true;
                    if (column[i] != "Кассир:" && column[i] != "Бухгалтер-назоратчи:")
                        a++;
                }
                ws.Column(6).Width = 25;
                ws.Column(7).Width = 30;
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] SecondToExport(List<ExcelModelForJournal16Second> model, string user)
        {
            try
            {
                int a = 0;
                string 
                    other = "",
                    worthless = "",
                    
                    fake = "",
                    excess = "",
                    total = "";
                foreach (var item in model)
                {
                    other += $"{item.OtherSumma} (" + item.OtherSummaText + ") " + item.CurrencyName + ", ";
                    //lack += $"{item.TotalFakeSumma}(" + item.TotalFakeSummaText + ")" + item.CurrencyName + ",";
                    worthless += $"{item.TotalWorthlessSumma}(" + item.TotalWorthlessSummaText + ") " + item.CurrencyName + ", ";
                    fake += $"{item.TotalFakeSumma} (" + item.TotalFakeSummaText + ") " + item.CurrencyName + ", ";
                    excess += $"{item.TotalExcessSumma} (" + item.TotalExcessSummaText + ") " + item.CurrencyName + ", ";
                    total += $"{item.TotalSumma} (" + item.TotalSummaText + ") " + item.CurrencyName + ", ";
                }

                
                
                string[] excelColumn =
                 {
                    $"IV.{model.FirstOrDefault().AcceptDate:dd.MM.yyyy} да банк кассасига инкассаторлар гуруҳидан қабул қилиб олинган инкассация сумкаларидаги (қопларидаги) нақд пул тушумлари қайта",
                    $"санаш кассаси кассирлари томонидан қайта санаб чиқилганидан:", other+ $"  сўм камомад (шу жумладан," +
                    worthless +$" тўловга яроқсиз," +
                    fake +$"қалбаки) пуллар;", $"" +
                    excess +$" ортиқча пуллар мавжудлиги аниқланди.",
                    $"Натижада, инкассаторлар гуруҳи томонидан банк (филиал)нинг бинодан ташқарида жойлашган кассаларидан йиғиб келинган _______________та ",
                    $"инкассация сумка (қоп)ларидаги ўраб-боғланган " +
                    total+$" нақд ",
                    $"(сумма рақам ва сўз билан)",
                    $"пуллар ва ________________________________________________________________________________________________________________ сўмлик",
                    $"қимматликлар, шунингдек,",
                    "(сумма рақам ва сўз билан)",
                    "мижозларнинг____________________________________________________________________________________та инкассация сумкаси (қопи)дан ",
                    "чиққан ва қайтасанаш кассаси ",
                    "кассирлари томонидан қайта",
                    "саналган________________________________________________________________________________________________________ сўмлик",
                    "(ҳақиқатда чиққан сумма рақам ва сўз билан)",
                    "нақд пуллар банк айланма кассасига кирим қилинди."
                };
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Book18Back");
                for (int i = 0; i < excelColumn.Length; i++)
                {
                    ws.Cells[$"B{i + 1}:F{i + 1}"].Value = $"{excelColumn[i]}";
                    ws.Cells[$"B{i + 1}:F{i + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[$"B{i + 1}:F{i + 1}"].Merge = true;
                    ws.Column(i + 1).Width = 30;
                    ws.Row(i + 1).Height = 25;

                    a++;
                }

                ws.Cells[$"E{a + 1}:F{a + 1}"].Value = $"Касса мудири: ________________________ _________";
                ws.Cells[$"E{a + 1}:F{a + 1}"].Style.Font.Bold = true;
                ws.Cells[$"E{a + 1}:F{a + 1}"].Merge = true;
                ws.Cells[$"E{a + 1}:F{a + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[$"E{a + 2}"].Value = $"Бош Ҳисобчи:";
                ws.Cells[$"E{a + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[$"F{a + 2}"].Value = model.FirstOrDefault().MainAccounterName;
                ws.Cells[$"F{a + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[$"E{a + 2}"].Style.Font.Bold = true;
                ws.Cells[$"F{a + 2}"].Style.Font.Bold = true;
                ws.Cells[$"F{a + 2}"].Style.Font.Bold = true;
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
