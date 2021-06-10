using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Models
{
    public class TokenModel
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public long expires_in { get; set; }
        public string permission { get; set; }
    }
}
