using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    /// <summary>
    /// 
    /// </summary>
    [Table("JOURNAL_110")]
    public class Journal110 : IEntity<int>
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Символ номи
        /// </summary>
        [Column("SYMBOL_NAME")]
        public string SymbolName { get; set; }

        /// <summary>
        /// Сони
        /// </summary>
        [Column("COUNT")]
        public int Count { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        [Column("SUMMA")]
        public double Summa { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("USER_ID")]
        public int UserId { get; set; }

        /// <summary>
        /// Сана
        /// </summary>
        [Column("DATE", TypeName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Bank kodi
        /// </summary>
        [Column("BANK_KOD")]
        public int BankCode { get; set; }

        /// <summary>
        /// Simvol kodi
        /// </summary>
        [Column("SYMBOL_KOD")]
        public int SymbolCode { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }
    }
}
