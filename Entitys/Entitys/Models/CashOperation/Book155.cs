using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Entitys.Models.CashOperation
{
    /// <summary>
    /// 
    /// </summary>
    [Table("BOOK_155")]
    public class Book155 : IEntity<int>
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
        public int FromCashierId { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        [Column("SPR_OBJECT_ID")]
        public int? SprObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("WORTH_ACCOUNT")]
        public string WorthAccount { get; set; }

        /// <summary>
        /// Sistema vaqti
        /// </summary>
        [Column("SYSTEM_DATE")]
        public DateTime? SystemDate { get; set; }

        /// <summary>
        /// Сановчи кассир коди
        /// </summary>        
        [Column("COUNTER_CASHIER_ID")]
        public int? CounterCashierId { get; set; }
        
        /// <summary>
        /// Қайта санаш кассасиданми
        /// </summary>
        [Column("FROM_175")]
        public bool From175 { get; set; }

        [Column("OTHER_INOUT_ID")]
        public int? OtherInoutId { get; set; }
    }
}
