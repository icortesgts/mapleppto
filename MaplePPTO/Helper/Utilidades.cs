using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaplePPTO.Helper
{
    public class Utilidades
    {
        public static string getStringFromPassword(byte[] password)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(password);
        }
    }
}