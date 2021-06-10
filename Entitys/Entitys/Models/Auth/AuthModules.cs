using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("auth_moduless")]
    public class AuthModules : IEntity<int>
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
        [Column("remark")]
        public string Remark { get; set; }
        /// <summary>
        /// Тегишли модул
        /// </summary>
        [Column("parent_id")]
        [ForeignKey("ModulModel")]
        public int? ParentId { get; set; }
        [IgnoreDataMember]
        public virtual AuthModules ModulModel { get; set; }
    }
}
