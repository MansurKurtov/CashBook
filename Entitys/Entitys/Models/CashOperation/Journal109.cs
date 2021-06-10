using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.CashOperation
{
    /// <summary>
    /// Кирим касса айланмаси
    /// </summary>
    [Table("JOURNAL_109")]
    public class Journal109 : IEntity<int>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Symbol nomi
        /// </summary>
        [Column("SYMBOL_NAME")]
        public string SymbolName { get; set; }

        /// <summary>
        /// Soni
        /// </summary>
        [Column("COUNT")]
        public int Count { get; set; }

        /// <summary>
        /// Summasi
        /// </summary>
        [Column("SUMMA")]
        public double Summa { get; set; }

        /// <summary>
        /// Foydalanuvchi Idisi
        /// </summary>
        [Column("USER_ID")]
        public int UserId { get; set; }

        /// <summary>
        /// Sana
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
    }
}
