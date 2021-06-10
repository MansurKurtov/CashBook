using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AuthService.Models
{
    public class LoginModel
    {
        [Required]
        public string login { get; set; }
        [Required]
        public string password { get; set; }
        /// <summary>
        /// device id
        /// </summary>
        public string clientId { get; set; }

    }

    public class PincodeLoginModel
    {
        public int StoreId { get; set; }
        public int CompanyId { get; set; }
        public string Pincode { get; set; }
    }
}
