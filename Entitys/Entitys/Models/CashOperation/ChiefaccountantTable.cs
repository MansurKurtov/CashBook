using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    /// <summary>
    /// 
    /// </summary>
    [Table("CHIEFACCOUNTANT")]
    public class ChiefaccountantTable : IEntity<int>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("FIO")]
        public string FIO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("BANK_KOD")]
        public int BankKod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CREATED_USER_ID")]
        public int CreatedUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("UPDATED_USER_ID")]
        public int? UpdatedUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}
