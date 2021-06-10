using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entitys.Models.CashOperation
{
    public class Reports
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("ISUSER")]
        public int IsUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("KOD")]
        public int Kod { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SUMMA")]
        public double? Summa { get; set; }
    }

    public class ReportsViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int IsUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Kod { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        public string Fio { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        public double? Summa { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public string Date { get; set; }
    }
    public class GetReport1Cashiers
    {
        public int ISUSER { get; set; }
        public int CASHIERID { get; set; }
        public string FIO { get; set; }
        public string POSITION { get; set; }

    }
    public class GetReport1Summs
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("ISUSER")]
        public int Isuser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CASHIERID")]
        public int CashierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SUMMA")]
        public double? Summa { get; set; }
    }
    public class GetReport2CurrencyCashiers
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("ISUSER")]
        public int Isuser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CASHIERID")]
        public int CashierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("POSITION")]
        public string Position { get; set; }
    }

    public class GetReport2CurrencySumms
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("ISUSER")]
        public int Isuser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CASHIERID")]
        public int CashierId { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        [Column("CURRENCYID")]
        public int CurrencyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SUMMA")]
        public double? Summa { get; set; }
    }
    public class GetReport2Currency
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("ISUSER")]
        public int Isuser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CASHIERID")]
        public int CashierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CASHIERFIO")]
        public string CashierFio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CURRENCYID")]
        public int CurrencyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CURRENCYNAME")]
        public string CurrencyName { get; set; }


    }
}
