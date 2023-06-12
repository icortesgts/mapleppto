using MaplePPTO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaplePPTO.Helper
{
    public class SesionModel
    {
        private static DatosSesionViewModel _DatosSesion;
        public static DatosSesionViewModel DatosSesion
        {
            get
            {
                var session = (DatosSesionViewModel)HttpContext.Current.Session[HelperConstantes.GSesionDatos];
                if (session == null) session = new DatosSesionViewModel();
                _DatosSesion = session;
                return _DatosSesion;
            }
            set
            {
                HttpContext.Current.Session[HelperConstantes.GSesionDatos] = value;
            }
        }

    }
}