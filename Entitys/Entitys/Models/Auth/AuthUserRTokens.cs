using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.Auth
{
    [Table("AUTH_USER_RTOKENS")]
    public class AuthUserRTokens : IEntity<int>
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ID")]
        public int Id { get; set; }


        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        [Column("USER_ID")]
        [ForeignKey("UserModel")]
        public int UserId { get; set; }

        public virtual AuthUsers UserModel { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        [Column("REFRESH_TOKEN")]
        public string RefreshToken { get; set; }


        /// <summary>
        /// Клиент коди(броузер, мобил)
        /// </summary>
        [Column("CLIENT_ID")]
        public string ClientId { get; set; }


        /// <summary>
        /// Яратилган сана
        /// </summary>
        [Column("CREATED")]
        public DateTime Created { get; set; }
        /// <summary>
        /// Токен янгиланган вакт
        /// </summary>
        [Column("UPDATED")]
        public DateTime Updated { get; set; }
    }
}
