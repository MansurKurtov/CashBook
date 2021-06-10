using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.PostModels.Auth
{
    public class RolePermissionPostModel
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
        /// Модул коди
        /// </summary>
        public int PermissionId { get; set; }
    }
}
