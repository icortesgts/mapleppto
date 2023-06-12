using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaplePPTO.Controllers
{
    public class GeneralController : Controller
    {
        public void CrearSesion(string name, string value)
        {
            Session[name] = value;
        }

        public JsonResult ObtenerSesion(string name)
        {
            var objSesion = Session[name];
            if (objSesion != null)
            {
                return Json(objSesion, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public static string RutaRaiz(HttpRequestBase request)
        {
            var host = request.ApplicationPath;
            if (host.Length > 1) host += "/";
            if (host == "/") host = "";
            return host;
        }

    }
}