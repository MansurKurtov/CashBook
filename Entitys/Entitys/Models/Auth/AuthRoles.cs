using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("auth_roles")]
    public class AuthRoles : IEntity<int>
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
        /// Изоҳ
        /// </summary>
        [Column("comment")]
        public string Comment { get; set; }


        /// <summary>
        /// Яратган фойдаланувчи
        /// </summary>
        [Column("createdby")]
        [ForeignKey("CreatedByModel")]
        public int CreatedBy { get; set; }

        [NotMapped]
        public virtual AuthUsers CreatedByModel { get; set; }



        /// <summary>
        /// Яратилган сана
        /// </summary>
        [Column("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Ташкилот коди
        /// </summary>
        [Column("structure_id")]
        public int StructureId { get; set; }
    }
}
