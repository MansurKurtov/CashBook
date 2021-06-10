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
    [Table("JOURNAL_176")]
    public class Journal176 : IEntity<int>
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
        /// Қоп рақами
        /// </summary>
        [Column("BAG_NUMBER")]
        public string BagNumber { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        [Column("SUMMA")]
        public double Summa { get; set; }

        /// <summary>
        /// Камомад сумма
        /// </summary>
        [Column("LACK_SUMMA")]
        public double LackSumma { get; set; }

        /// <summary>
        /// Яроқсиз сумма
        /// </summary>
        [Column("WORTHLESS_SUMMA")]
        public double WorthlessSumma { get; set; }

        /// <summary>
        /// Чеклар сони
        /// </summary>
        [Column("RECEIPT_COUNT")]
        public int ReceiptCount { get; set; }

        /// <summary>
        /// Чеклар суммаси
        /// </summary>
        [Column("RECEIPT_SUMMA")]
        public double ReceiptSumma { get; set; }

        /// <summary>
        /// Изоҳ
        /// </summary>
        [Column("COMMENT")]
        public string Comment { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("USER_ID")]
        public int UserId { get; set; }

        /// <summary>
        /// Сановчи кассир
        /// </summary>
        [Column("COUNTER_CASHIER_ID")]
        [ForeignKey("CounterCashierModel")]
        public int? CounterCashierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        public virtual CounterCashier CounterCashierModel { get; set; }

        /// <summary>
        /// Ортиқча сумма
        /// </summary>
        [Column("EXCESS_SUMMA")]
        public double ExcessSumma { get; set; }

        /// <summary>
        /// Сервер вақти
        /// </summary>
        [Column("REAL_TIME")]
        public DateTime RealTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("STATUS")]
        public int Status { get; set; } = 0;

        /// <summary>
        /// Журнал - 16 даги сана
        /// </summary>
        [Column("DATE_16", TypeName ="date")]
        public DateTime Date16 { get; set; }

        /// <summary>
        /// Қалбаки пуллар суммаси
        /// </summary>
        [Column("FAKE_SUMMA")]
        public double FakeSumma { get; set; }

        /// <summary>
        /// Валюта коди
        /// </summary>
        [Column("SPR_OBJECT_ID")]
        public int SprObjectId { get; set; }
    }
}
