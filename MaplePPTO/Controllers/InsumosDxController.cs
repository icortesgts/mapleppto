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
    public class InsumosDxController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: InsumosDxes
        public async Task<ActionResult> Index()
        {
            return View(await db.InsumosDx.ToListAsync());
        }

        // GET: InsumosDxes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InsumosDxes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,descripcion,presentacion,basl_leves,titulacion,nd,poligrafia,autoTitutacion,nivel2,fecha_crea,fecha_modifica,usuario_crea,usuario_modifica")] InsumosDx insumosDx)
        {
            if (ModelState.IsValid)
            {
                db.InsumosDx.Add(insumosDx);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(insumosDx);
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                InsumosDx insumosDx = await db.InsumosDx.FindAsync(id);
                db.InsumosDx.Remove(insumosDx);
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
