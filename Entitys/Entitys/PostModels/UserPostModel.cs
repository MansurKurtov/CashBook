using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entitys.PostModels
{
    /// <summary>
    /// 
    /// </summary>
    public class UserPostModel
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Фойдаланувчи логини
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Parol
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Telefon
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Bank kodi
        /// </summary>
        public int? BankId { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Administratormi?
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// kassir tipi idisi
        /// </summary>
        public int? CashierTypeId { get; set; }

        /// <summary>
        /// Permission Idlar
        /// </summary>
        public List<int> PermissionIds { get; set; }
    }
    public class UserChangePasswordModel
    {
        [BindNever]
        public int UserId { get; set; }
        /// <summary>
        /// Old password
        /// </summary>
        //[StringLength(25, MinimumLength = 6)]
        public string OldPassword { get; set; }

        /// <summary>
        /// New password
        /// 
        /// </summary>
        [StringLength(25, MinimumLength = 6)]
        public string NewPassword { get; set; }

    }

    public class UserRecoverPasswordModel
    {
        //[BindNever]
        public int UserId { get; set; }

        /// <summary>
        /// New password
        /// 
        /// </summary>
        [StringLength(25, MinimumLength = 6)]
        public string NewPassword { get; set; }

    }

}
