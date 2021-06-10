using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    [Table("SUP_ACCOUNTANT")]
    public class SupervisingAccountant : IEntity<int>
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// FIO
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }

        /// <summary>
        /// Bank kodi
        /// </summary>
        [Column("BANK_CODE")]
        public int BankCode { get; set; }

        /// <summary>
        /// Yozuvni qo'shgan foydalanuvchi nomi
        /// </summary>
        [Column("CREATED_USER_ID")]
        public int CreatedUserId { get; set; }

        /// <summary>
        /// Yozuv qo'shilgan vaqt
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// O'gartirgan foydalanuvchi
        /// </summary>
        [Column("UPDATED_USER_ID")]
        public int? UpdatedUserId { get; set; }

        /// <summary>
        /// O'zgartirilgan vaqti
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}
