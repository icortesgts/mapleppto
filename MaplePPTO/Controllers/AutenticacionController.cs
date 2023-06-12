using MaplePPTO.Helper;
using MaplePPTO.Models;
using MaplePPTO.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MaplePPTO.Controllers
{
    public class AutenticacionController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();
        // GET: Autenticacion
        public ActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel
            {
                Usuario = "icortesw",//string.Empty,
                Clave = "Majo!1921"//string.Empty
            };
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                var host = GeneralController.RutaRaiz(Request);
                model.Code = 0;
                if (!string.IsNullOrEmpty(model.Usuario))
                {
                    var usx =  db.Usuarios.Where(x => x.login == model.Usuario).Select(x => x).FirstOrDefault();

                    if (usx != null)
                    {
                        string pwd = Utilidades.getStringFromPassword(usx.Password);

                        if(pwd.Equals(model.Clave))
                        {
                            Perfil px = new Perfil();
                            px = db.Perfil.Where(x => x.id == usx.id_perfil).FirstOrDefault();
                            var sesioModel = new DatosSesionViewModel
                            {
                                usuario = usx.Nombre,
                                correo = usx.Email,
                                id_usuario = usx.Id,
                                Perfil = px.Nombre,
                                id_Perfil = px.id.ToString(),
                            };

                            SesionModel.DatosSesion = sesioModel;
                            model.ReturnURL = model.ReturnURL ?? host + "Home/Index";

                            string cookiestr;
                            FormsAuthentication.Initialize();
                            FormsAuthenticationTicket fat = new FormsAuthenticationTicket(1, sesioModel.usuario, DateTime.Now.AddHours(2), DateTime.Now.AddHours(2).AddMinutes(FormsAuthentication.Timeout.TotalMinutes), true, sesioModel.Perfil, FormsAuthentication.FormsCookiePath);
                            cookiestr = FormsAuthentication.Encrypt(fat);
                            HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                            if (model.Recordar)
                                ck.Expires = fat.Expiration;
                            ck.Path = FormsAuthentication.FormsCookiePath;
                            Response.Cookies.Add(ck);
                        }
                        else
                        {
                            model.Code = 14;
                            model.Message = "Usuario o Clave incorectos";
                        }

                    }
                    else
                    {
                        model.Code = 14;
                        model.Message = "Usuario o Clave incorectos";
                    }
                }
                else
                {
                    model.Code = 14;
                    model.Message = "Usuario o Clave incorectos";
                }


            }
            catch (Exception ex)
            {
                model.Code = 14;
                model.Message = "An error ocurred when try login";
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            SesionModel.DatosSesion = null;

            return RedirectToAction("Login", "Autenticacion");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}