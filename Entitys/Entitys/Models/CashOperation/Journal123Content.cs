using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.CashOperation
{
    [Table("JOURNAL_123_CONTENT")]
    public class Journal123Content : IEntity<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("SYSTEM_DATE")]
        public DateTime SystemDate { get; set; }

        [Column("BANK_KOD")]
        public int BankCode { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("COUNT")]
        public int Count { get; set; }

        [Column("VALUE")]
        public string Value { get; set; }

        [Column("SUMMA")]
        public double Summa { get; set; }

        [Column("TARGET")]
        public string Target { get; set; }

        [Column("JOURNAL_123_ID")]
        public int Journal123Id { get; set; }

        [Column("FIO")]
        public string Fio { get; set; }
    }
}
