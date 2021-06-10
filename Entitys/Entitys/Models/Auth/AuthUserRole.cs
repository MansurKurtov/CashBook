using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("AUTH_USER_ROLES")]
    public class AuthUserRole : IEntity<int>
    {


        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ID")]
        public int Id { get; set; }


        /// <summary>
        /// Рол кодиdasd 
        /// </summary>
        [Column("ROLE_ID")]
        [ForeignKey("AuthRolesModel")]
        public int RoleId { get; set; }

        public virtual AuthRoles AuthRolesModel { get; set; }


        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("USER_ID")]
        [ForeignKey("UsersModel")]
        public int UserId { get; set; }

        public virtual AuthUsers UsersModel { get; set; }
    }
}
