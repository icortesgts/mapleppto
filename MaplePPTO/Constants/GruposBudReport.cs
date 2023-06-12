using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaplePPTO.Constants
{
    public static class GruposBudReport
    {
        public static class Revenue
        {
            public static string Nombre = "REVENUE";
            public static readonly Dictionary<int, string> tables = new Dictionary<int, string>{{ 1, "Clients" } };
        }

        public static class DirectCost
        {
            public static string Nombre = "DIRECT COST";
            public static readonly Dictionary<int, string> tables = new Dictionary<int, string> { { 1, "Item" } };
        }

        public static class GeneralExpenses
        {
            public static string Nombre = "GENERAL EXPENSES";
            public static string Ips = "IPS EXPENSES";
            public static string Head = "HEAD OFFICE EXPENSES";
            public static readonly Dictionary<int, string> tables = new Dictionary<int, string>{
                    { 1, "Staff Expenses" },{2, "Operational Expenses" },{3, "Facilities Expenses" },
                    { 4, "Staff Expenses" },{5, "Operational Expenses" },{6, "Facilities Expenses" },{7, "Non Operational Income" },
                    { 8, "Non Operational Expenses" },{9, "Financial Expenses" },{10, "Fiscal Expenses" },{11, "Taxes" }};


        }


    }
}