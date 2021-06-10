using RepositoryCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    /// <summary>
    /// 
    /// </summary>
    [Table("COUNTER_CASHIER")]
    public class CounterCashier : IEntity<int>
    {
        /// <summary>
        /// Yozuv kodi
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Nomi
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Bank kodi
        /// </summary>
        [Column("BANK_KOD")]
        public int BankCode { get; set; }

        /// <summary>
        /// faolligi
        /// </summary>
        [Column("ACTIVE")]
        public bool Active { get; set; }
    }
}
