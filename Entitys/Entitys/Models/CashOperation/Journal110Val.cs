﻿using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.CashOperation
{
    [Table("JOURNAL_110_VAL")]
    public class Journal110Val : IEntity<int>
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Символ номи
        /// </summary>
        [Column("SYMBOL_NAME")]
        public string SymbolName { get; set; }

        /// <summary>
        /// Сони
        /// </summary>
        [Column("COUNT")]
        public int Count { get; set; }

        /// <summary>
        /// Сумма   
        /// </summary>
        [Column("SUMMA")]
        public double Summa { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("USER_ID")]
        public int UserId { get; set; }

        /// <summary>
        /// Сана
        /// </summary>
        [Column("DATE", TypeName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Банк коди
        /// </summary>
        [Column("BANK_KOD")]
        public int BankKod { get; set; }

        /// <summary>
        /// Символ коди
        /// </summary>
        [Column("SYMBOL_KOD")]
        public int SymbolKod { get; set; }

        /// <summary>
        /// ФИО 
        /// </summary>
        [Column("FIO")]
        public string Fio { get; set; }

        /// <summary>
        /// Валюта коди
        /// </summary>
        [Column("VALUT_KOD")]
        public int ValutKod { get; set; }

        /// <summary>
        /// Валюта номи
        /// </summary>
        [Column("VALUT_NAME")]
        public string ValutName { get; set; }
    }
}
