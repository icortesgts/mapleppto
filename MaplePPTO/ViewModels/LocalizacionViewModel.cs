using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaplePPTO.ViewModels
{
    public class LocalizacionViewModel
    {
        public long id { get; set; }
        public string coddpto { get; set; }
        public string NomDpto { get; set; }
        public string ISODpto { get; set; }
        public string CodMun { get; set; }
        public string NomMun { get; set; }
        public long id_pais { get; set; }
        public Nullable<bool> Operacion { get; set; }
        public List<SelectListItem> selectListPaices { get; internal set; }
        public List<SelectListItem> selectListDepartamentos { get; internal set; }
    }
}