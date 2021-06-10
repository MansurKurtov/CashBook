using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("auth_role_permissions")]
    public class AuthRolePermissions : IEntity<int>
    {


        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }


        /// <summary>
        /// Рол коди
        /// </summary>
        [Column("role_id")]
        [ForeignKey("AuthRolesModel")]
        public int RoleId { get; set; }

        public virtual AuthRoles AuthRolesModel { get; set; }


        /// <summary>
        /// Модул коди
        /// </summary>
        [Column("permission_id")]
        [ForeignKey("PermissionModel")]
        public int PermissionId { get; set; }

        public virtual AuthPermissions PermissionModel { get; set; }
    }
}
