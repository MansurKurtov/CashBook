using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.Enums
{
    [Table("ENT_OPERATION_155")]
    public class EntOperations155 : IEntity<int>
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
