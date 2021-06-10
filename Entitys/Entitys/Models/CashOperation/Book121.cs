using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    [Table("BOOK_121")]
    public class Book121:IEntity<int>
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Сана
        /// </summary>
        [Column("DATE", TypeName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Кун бошидаги сальдо
        /// </summary>
        [Column("SALDO_BEGIN_COUNT")]
        public int SaldoBeginCount { get; set; }

        /// <summary>
        /// Кирим
        /// </summary>
        [Column("INCOME_COUNT")]
        public int? InComeCount { get; set; }

        /// <summary>
        /// Чиқим
        /// </summary>
        [Column("OUTGO_COUNT")]
        public int? OutGoCount { get; set; }

        /// <summary>
        /// Кун охиридаги сальдо
        /// </summary>
        [Column("SALDO_END_COUNT")]
        public int? SaldoEndCount { get; set; }

        /// <summary>
        /// Валюта коди
        /// </summary>
        [Column("KOD_VALUT")]
        public int KodValut { get; set; }

        /// <summary>
        /// Bank kodi
        /// </summary>
        [Column("BANK_KOD")]
        public int BankCode { get; set; }

        /// <summary>
        /// Kirim summasi
        /// </summary>
        [Column("INCOME_SUMMA")]
        public double? IncomeSumma { get; set; }

        /// <summary>
        /// Chiqim summasi
        /// </summary>
        [Column("OUTGO_SUMMA")]
        public double? OutgoSumma { get; set; }

        /// <summary>
        /// Kun boshidagi saldo summasi
        /// </summary>
        [Column("SALDO_BEGIN_SUMMA")]
        public double SaldoBeginSumma { get; set; }

        /// <summary>
        /// Kun oxiridagi saldo summasi
        /// </summary>
        [Column("SALDO_END_SUMMA")]
        public double? SaldoEndSumma { get; set; }
    }
}
