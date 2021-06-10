using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("auth_uielements")]
    public class AuthUIElements : IEntity<int>
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
        [Column("element_code")]
        public string ElementCode { get; set; }


        /// <summary>
        /// Изоҳ
        /// </summary>
        [Column("comment")]
        public string Comment { get; set; }

    }
}
