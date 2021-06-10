using Entitys.Models.Reference;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("auth_structure_permissins")]
    public class AuthStructurePermission : IEntity<int>
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Структура коди
        /// </summary>
        [Column("structure_id")]
        [ForeignKey("RefStructureModel")]
        [Required]
        public int StructureId { get; set; }
        [IgnoreDataMember]
        public virtual RefStructure RefStructureModel { get; set; }
        /// <summary>
        /// Permission кодлар
        /// </summary>
        [Column("permission_codes")]
        [Required]
        public string PermissionCodes { get; set; }

        /// <summary>
        /// Permission кодлар
        /// </summary>
        [Column("uielement_codes")]
        [Required]
        public string UIelementCodes { get; set; }
    }
}
