using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.Helper.UserName
{
    public class PutUserName
    {
        public static string GetPutUser(string FirstName, string MiddleName,string LastName) 
        {
            var user = LastName;
            if (string.IsNullOrEmpty(FirstName) == false)
                user = user + "." + FirstName[0];
            if (string.IsNullOrEmpty(MiddleName) == false)
                user = user + "." + MiddleName[0];
            return user;
        }
    }
}
