using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    [Table("BOOK_120")]
    public class Book120 : IEntity<int>
    {
        /// <summary>
        /// Yozuv kodi
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Sana
        /// </summary>
        [Column("DATE", TypeName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Kun boshidagi saldo
        /// </summary>
        [Column("SALDO_BEGIN_COUNT")]
        public int SaldoBeginCount { get; set; }

        /// <summary>
        /// Kirim
        /// </summary>
        [Column("INCOME_COUNT")]
        public int? IncomeCount { get; set; }

        /// <summary>
        /// Chiqim
        /// </summary>
        [Column("OUTGO_COUNT")]
        public int? OutgoCount { get; set; }

        /// <summary>
        /// Kun oxiridagi saldo
        /// </summary>
        [Column("SALDO_END_COUNT")]
        public int? SaldoEndCount { get; set; }

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
