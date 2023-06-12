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
    public class EscenarioController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: Escenario
        public async Task<ActionResult> Index()
        {
            return View(await db.Escenario.ToListAsync());
        }

 // GET: Escenario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Escenario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Nombre,Descripcion")] Escenario escenario)
        {
            if (ModelState.IsValid)
            {
                db.Escenario.Add(escenario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(escenario);
        }


        // POST: Escenario/Delete/5

        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                Escenario escenario = await db.Escenario.FindAsync(id);
                db.Escenario.Remove(escenario);
                await db.SaveChangesAsync();
                var response = new
                {
                    code = 0,
                    message = ""
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
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
