using RepositoryCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.Enums
{
    [Table("TYPE_CASHIERS")]
    public class CashierType : IEntity<int>
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
    }
}
