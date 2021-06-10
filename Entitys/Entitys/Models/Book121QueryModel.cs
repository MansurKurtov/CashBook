using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models
{
    public class Book121QueryModel
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
        [Column("SALDO_BEGIN_SONI")]
        public int? SaldoBeginSoni { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SALDO_BEGIN_SUMMA")]
        public double? SaldoBeginSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("KIRIM_SONI")]
        public int? KirimSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("KIRIM_SUMMA")]
        public double? KirimSumma{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CHIQIM_SONI")]
        public int? ChiqimSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CHIQIM_SUMMA")]
        public double? ChiqimSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SALDO_END_SONI")]
        public int? SaldoEndSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SALDO_END_SUMMA")]
        public double? SaldoEndSumma { get; set; }
    }

    public class Book171QueryModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("ACCOUNT")]
        public string Account { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        [Column("ACCOUNT_SHORT")]
        public string AccountShort { get; set; } 

        /// <summary>
        /// Symbol nomi
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        [Column("SALDO_BEGIN_SONI")]
        public int? SaldoBeginSoni { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SALDO_BEGIN_SUMMA")]
        public double? SaldoBeginSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("KIRIM_SONI")]
        public int? KirimSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("KIRIM_SUMMA")]
        public double? KirimSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CHIQIM_SONI")]
        public int? ChiqimSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CHIQIM_SUMMA")]
        public double? ChiqimSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SALDO_END_SONI")]
        public int? SaldoEndSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SALDO_END_SUMMA")]
        public double? SaldoEndSumma { get; set; }
    }
    public class Book121Excel
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
        /// Summasi
        /// </summary>
        public double? SaldoBeginSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? KirimSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? KirimSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ChiqimSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? ChiqimSumma { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public double? SaldoEndSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BoshKassir { get; set; }
    }

    public class Book171Excel
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// Symbol nomi
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        public double? SaldoBeginSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? KirimSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? KirimSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ChiqimSoni { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? ChiqimSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? SaldoEndSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BoshKassir { get; set; }
    }
}
