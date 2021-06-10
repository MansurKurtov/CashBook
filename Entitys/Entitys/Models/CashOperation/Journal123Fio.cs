using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.CashOperation
{
    [Table("JOURNAL_123_FIO")]
    public class Journal123Fio : IEntity<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("JOURNAL_123_ID")]
        public int Journal123Id { get; set; }

        [Column("FIO")]
        public string Fio { get; set; }
    }
}
