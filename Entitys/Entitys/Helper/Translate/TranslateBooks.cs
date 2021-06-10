using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.Helper.Translate
{
    public class TranslateBooks
    {
        public static string GetTranslateBook(string book) 
        {
            string translateBook=null;
            if (book == "BALANCE_BEGIN")
                translateBook = "Кун бошига сальдо";
            if (book == "INCOME")
                translateBook = "Кирим";
            if (book == "OUTGO")
                translateBook = "Чиқим";
            if (book == "BALANCE_END")
                translateBook = "Кун охирига сальдо";
            return translateBook ;
        }
    }
}
