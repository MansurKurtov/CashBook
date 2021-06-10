using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.Auth
{
    public class RolePermissionViewModel
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
        /// 
        [SwaggerIgnore]
        public string RoleName { get; set; }

        public int PermissionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SwaggerIgnore]
        public string PermissionName { get; set; }

        //public string ActionCodes { get; set; }

    }
}
