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
    [Table("COLLECTOR")]
    public class Collector : IEntity<int>
    {
        /// <summary>
        /// Yozuv kodi
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
        /// 
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SYSTEM_DATE")]
        public DateTime SystemDate { get; set; }
    }
}
