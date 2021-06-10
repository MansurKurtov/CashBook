using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    /// <summary>
    /// 
    /// </summary>
    [Table("JOURNAL_111")]
    public class Journal111 : IEntity<int>
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
        /// Банк коди
        /// </summary>
        [Column("BANK_KOD")]
        public int BankKod { get; set; }

        /// <summary>
        /// Ички саралаш учун махсус сон
        /// </summary>
        [Column("OPER_KOD")]
        public int OperKod { get; set; }

        /// <summary>
        ///  Сони
        /// </summary>
        [Column("COUNT")]
        public int Count { get; set; }

        /// <summary>
        ///Сумма
        /// </summary>
        [Column("SUMMA")]
        public double Summa { get; set; }

        /// <summary>
        /// Ички саралаш учун махсус сўз
        /// </summary>
        [Column("OPER_NAME")]
        public string OperName { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("USER_ID")]
        public int UserId { get; set; }
    }
}
