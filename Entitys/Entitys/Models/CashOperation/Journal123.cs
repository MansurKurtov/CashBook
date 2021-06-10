using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    [Table("JOURNAL_123")]
    public class Journal123 : IEntity<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        [Column("DATE", TypeName = "date")]
        public DateTime Date { get; set; }

        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("SYSTEM_DATE")]
        public DateTime SystemDate { get; set; }
    }
}
