using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("auth_user_permissions")]
    public class AuthUserPermissions : IEntity<int>
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }


        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("user_id")]
        [ForeignKey("UsersModel")]
        public int UserId { get; set; }

        public virtual AuthUsers UsersModel { get; set; }


        /// <summary>
        /// Модул коди
        /// </summary>
        [Column("permission_id")]
        [ForeignKey("PermissionModel")]
        public int PermissionId { get; set; }

        public virtual AuthPermissions PermissionModel { get; set; }
    }
}
