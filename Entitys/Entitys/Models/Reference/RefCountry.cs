using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.Reference
{
    [Table("ref_countries")]
    public class RefCountry : IEntity<int>
    {

        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Номи
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Қисқача коди
        /// </summary>
        [Column("code")]
        public string Code { get; set; }

        /// <summary>
        /// Display order
        /// </summary>
        [Column("display_order")]
        public int DisplayOrder { get; set; }
    }
}
