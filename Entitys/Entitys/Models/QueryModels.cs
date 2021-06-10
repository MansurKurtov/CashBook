using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models
{
    public class QueryModel
    {
        [Column("KOD")]
        public int Id { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        [Column("NAME")]
        public string SymbolName { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        [Column("SONI")]
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SUMMA")]
        public double? Summa { get; set; }
    }
    public class QueryModelForJournal111
    {

        public int Id { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>        
        public string SymbolName { get; set; }

        /// <summary>
        /// Soni
        /// </summary>        
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>        
        public double? Summa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OrignText { get; set; }
    }
    public class QueryModels
    {
        [Column("KOD")]
        public int Id { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        [Column("SONI")]
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SUMMA")]
        public double Summa { get; set; }


    }    
    public class ExcelModel
    {
        
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        public string Kod { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        public double Summa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BoshKassir { get; set; }

        /// <summary>
        /// Fio
        /// </summary>
        public string Fio { get; set; }


    }

    public class Jourlan110ReturnExcelModel
    {
        public List<Journal110ReturnExcelModel> Data { get; set; }
        public string HeadCashier { get; set; }
    }

    public class Journal110ReturnExcelModel
    {
        public int Count { get; set; }
        public int Grouped { get; set; }
        public int Position { get; set; }
        public bool Show { get; set; }
        public int ValutSoni { get; set; }
        public double ValutSummasi { get; set; }
        public List<Journal110ReturnItem> Value { get; set; }
    }

    public class Journal110ReturnItem
    {
        public int Count { get; set; }
        public string Fio { get; set; }
        public string Kod { get; set; }
        public int Summa { get; set; }
        public int SymbolKod { get; set; }
        public string SymbolName { get; set; }
        public int ValutKod { get; set; }
        public string ValutName { get; set; }
    }

    public class Journal110WorthExcelModel
    {
        public List<Journal110WorthGroupData> Data { get; set; }
        public double CashValueSum { get; set; }
        public string HeadCashier { get; set; }
    }

    public class Journal110WorthGroupData
    {
        public int Count { get; set; }
        public int Grouped { get; set; }
        public string Key { get; set; }
        public int Position { get; set; }
        public bool Show { get; set; }
        public List<Journal110WorthItem> Value { get; set; }
    }

    public class Journal110WorthItem
    {
        public int Count { get; set; }
        public string Fio { get; set; }
        public string Id { get; set; }
        public string Kod { get; set; }
        public string Name { get; set; }
        public double Summa { get; set; }
    }

    public class Journal110ExcelModel
    {
        public List<ExcelModel> Data { get; set; }
        public double CashValue { get; set; }
        public string HeadCashierName { get; set; }
    }
    public class QueryModelFor109Worth
    {
        [Column("KOD")]
        public string Id { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// FIO
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        [Column("SONI")]
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SUMMA")]
        public double Summa { get; set; }


    }
    public class GetWorkingDates
    {
        public DateTime? RESULT { get; set; }
    }

    public class Journal109ValQueryModel
    {
        [Column("SYMBOL_KOD")]
        public int SymbolKod { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        [Column("SYMBOL_NAME")]
        public string SymbolName { get; set; }

        /// <summary>
        /// Valyuta kodi
        /// </summary>
        [Column("VALUT_KOD")]
        public int ValutKod { get; set; }

        /// <summary>
        /// Valyuta nomi
        /// </summary>
        [Column("VALUT_NAME")]
        public string ValutName { get; set; }

        /// <summary>
        /// FIO
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        [Column("SONI")]
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SUMMA")]
        public double? Summa { get; set; }
    }

    public class Journal110ValQueryModel
    {
        [Column("SYMBOL_KOD")]
        public int SymbolKod { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        [Column("SYMBOL_NAME")]
        public string SymbolName { get; set; }

        /// <summary>
        /// Valyuta kodi
        /// </summary>
        [Column("VALUT_KOD")]
        public int ValutKod { get; set; }

        /// <summary>
        /// Valyuta nomi
        /// </summary>
        [Column("VALUT_NAME")]
        public string ValutName { get; set; }

        /// <summary>
        /// FIO
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        [Column("SONI")]
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SUMMA")]
        public double? Summa { get; set; }
    }


    public class ExcelModelForBook141
    {

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Symbol nomi
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        public double Summa { get; set; }

        public string BoshKassir { get; set; }


    }
    public class ExcelFor123Froms 
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Summa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Journal123Fio { get; set; }
    }
    public class ExcelModel111
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Symbol nomi
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        public double Summa { get; set; }


    }

    public class ExcelModelForJournal16
    {
        /// <summary>
        /// 
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        ///Йўналиш рақами 
        /// </summary>
        public string DirectionNumber { get; set; }

        /// <summary>
        /// Сана
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Инкассация ФИО лари
        /// </summary>
        public string CollectorName { get; set; }
        

        /// <summary>
        /// Қоплар сони
        /// </summary>
        public int BagsCount { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public double Summa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuperVisorName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? AcceptDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CashierName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Summauz { get; set; }
        public List<Journal16CurrencyModel> CurrencyList { get; set; }
    }

    public class Journal16CurrencyModel
    {
        public int Kod { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int Count { get; set; }
        public string WordName { get; set; }
    }

    public class ExcelModelForJournal16Second 
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime AcceptDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double TotalLackSumma { get; set; }

        public string TotalLackSummaText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double TotalWorthlessSumma { get; set; }
        public string TotalWorthlessSummaText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double TotalFakeSumma { get; set; }

        public string TotalFakeSummaText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double TotalExcessSumma { get; set; }
        public string TotalExcessSummaText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double TotalSumma { get; set; }
        public string TotalSummaText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MainAccounterName { get; set; }

        public double OtherSumma { get; set; }
        public string OtherSummaText { get; set; }

        public string CurrencyName { get; set; }

    }
    public class ExcelModelForJournal176
    {

        /// <summary>
        /// Сана
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Қоп рақами
        /// </summary>
        public string BagNumber { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public double Summa { get; set; }

        /// <summary>
        /// Камомад сумма
        /// </summary>
        public double LackSumma { get; set; }

        /// <summary>
        /// Яроқсиз сумма
        /// </summary>
        public double WorthlessSumma { get; set; }

        /// <summary>
        /// Чеклар сони
        /// </summary>
        public int ReceiptCount { get; set; }

        /// <summary>
        /// Чеклар суммаси
        /// </summary>
        public double ReceiptSumma { get; set; }

        /// <summary>
        /// Изоҳ
        /// </summary>
        public string Comment { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string CounterCashierName { get; set; }

        /// <summary>
        /// Ортиқча сумма
        /// </summary>
        public decimal ExcessSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BoshKassir { get; set; }

        /// <summary>
        /// Valyuta nomi
        /// </summary>
        public string CurrencyName { get; set; }
    }
}
