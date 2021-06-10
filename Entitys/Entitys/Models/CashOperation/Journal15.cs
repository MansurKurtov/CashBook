
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
    [Table("JOURNAL_15")]
    public class Journal15 : IEntity<int>
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Йўналиш рақами
        /// </summary>
        [Column("JOURNAL_16_ID")]
        public int Journal16Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        public virtual Journal16 Book16Model { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        [Column("SUMMA")]
        public double Summa { get; set; }

        /// <summary>
        /// Изоҳ
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("USER_ID")]
        public int UserId { get; set; }

        /// <summary>
        /// Қоп раками
        /// </summary>
        [Column("BAGS_NUMBER")]
        public string BagsNumber { get; set; }

        /// <summary>
        /// Қоп бўшми
        /// </summary>
        [Column("IS_EMPTY")]
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Система вақти
        /// </summary>
        [Column("SYSTEM_DATE")]
        public DateTime SystemDate { get; set; }

        /// <summary>
        /// Валюта коди
        /// </summary>
        [Column("SPR_OBJECT_ID")]
        public int SprObjectId { get; set; }
    }
}
