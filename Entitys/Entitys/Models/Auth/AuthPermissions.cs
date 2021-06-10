using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("auth_permissions")]
    public class AuthPermissions : IEntity<int>
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
        /// Permission
        /// </summary>
        [Column("permission_name")]
        public string PermissionName { get; set; }
        /// <summary>
        /// Permission
        /// </summary>
        [Column("permission_code")]
        public string PermissionCode { get; set; }


        /// <summary>
        /// Тегишли модул
        /// </summary>
        [Column("module_id")]
        [ForeignKey("ModulModel")]
        public int ModulId { get; set; }

        [IgnoreDataMember]
        public virtual AuthModules ModulModel { get; set; }

        /// <summary>
        /// Display order
        /// </summary>
        [Column("display_order")]
        public string DisplayOrder { get; set; }
        /// <summary>
        /// Permission га тегишли UI element кодлар
        /// </summary>
        [Column("related_uielement_codes")]
        public string RelatedUielementCodes { get; set; }
        /// <summary>
        /// Permission га тегишли permission кодлар
        /// </summary>
        [Column("related_permission_codes")]
        public string RelatedPermissionCodes { get; set; }
        /// <summary>
        /// Изоҳ
        /// </summary>
        [Column("comment")]
        public string Comment { get; set; }
    }
}
