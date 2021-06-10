using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.Auth
{
    public class UserRoleViewModel
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Рол коди
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        public int UserId { get; set; }

    }
}
