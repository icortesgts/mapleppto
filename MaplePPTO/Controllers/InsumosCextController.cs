using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MaplePPTO.Models;

namespace MaplePPTO.Controllers
{
    public class InsumosCextController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: InsumosCext
        public async Task<ActionResult> Index()
        {
            return View(await db.InsumosCext.ToListAsync());
        }

        // GET: InsumosCext/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InsumosCext/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,descripcion,presentacion,mExperto,somnlogo,tRespiratorio,nutricion,psicologia,fecha_crea,fecha_modifica,usuario_crea,usuario_modifica")] InsumosCext insumosCext)
        {
            if (ModelState.IsValid)
            {
                db.InsumosCext.Add(insumosCext);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(insumosCext);
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                InsumosCext insumosCext = await db.InsumosCext.FindAsync(id);
                db.InsumosCext.Remove(insumosCext);
                await db.SaveChangesAsync();

                var response = new
                {
                    code = 0,
                    message = ""
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var response = new
                {
                    code = 10,
                    message = "An error ocurred!"
                };

                return Json(response, JsonRequestBehavior.AllowGet);

            }

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
