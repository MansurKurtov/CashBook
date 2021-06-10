using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    /// <summary>
    /// 
    /// </summary>
    [Table("JOURNAL_16")]
    public class Journal16 : IEntity<int>
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        ///Йўналиш рақами 
        /// </summary>
        [Column("DIRECTION_NUMBER")]
        public string DirectionNumber { get; set; }

        /// <summary>
        /// Сана
        /// </summary>
        [Column("DATE", TypeName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("USER_ID")]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SYSTEM_DATE")]
        public DateTime SystemDate { get; set; }

        [Column("ACCEPT")]
        public bool Accept { get; set; } = false;

        [Column("ACCEPT_DATE")]
        public DateTime? AcceptDate { get; set; }

        [Column("STATUS")]
        public int Status { get; set; } = 0;
    }
}
