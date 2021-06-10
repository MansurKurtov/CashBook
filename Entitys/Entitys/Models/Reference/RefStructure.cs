using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.Reference
{
    [Table("ref_structures")]
    public class RefStructure : IEntity<int>
    {

        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Объектнинг қисқача коди
        /// </summary>
        [Column("name")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Изоҳ
        /// </summary>
        [Column("comment")]
        public string Comment { get; set; }
    }
}
