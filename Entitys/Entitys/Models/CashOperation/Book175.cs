using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    [Table("JOURNAL_175")]
    public class Book175 : IEntity<int>
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
        /// Кассир коди(топширадиган)
        /// </summary>
        [Column("FROM_CASHIER_ID")]
        public int FromCasheirId { get; set; }

        /// <summary>
        /// Кассир коди(қабул қиладиган)
        /// </summary>
        [Column("TO_CASHIER_ID")]
        public int ToCashierId { get; set; }

        /// <summary>
        /// Қиймати
        /// </summary>
        [Column("CASH_VALUE")]
        public double CashValue { get; set; }

        /// <summary>
        /// Олдим
        /// </summary>
        [Column("ACCEPT")]
        public bool Accept { get; set; }

        /// <summary>
        /// Номинал тури
        /// </summary>
        [Column("OPERATION_ID")]
        public int OperationId { get; set; }

    }
}
