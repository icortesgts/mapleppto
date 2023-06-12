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
    public class ConceptoController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: Conceptoes
        public async Task<ActionResult> Index()
        {
            var concepto = db.Concepto.Include(c => c.Fase);
            return View(await concepto.ToListAsync());
        }


        // GET: Conceptoes/Create
        public ActionResult Create()
        {
            ViewBag.id_fase = new SelectList(db.Fase, "id", "Nombre");
            return View();
        }

        // POST: Conceptoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Nombre,Name,Entrada,TipoPpto,id_fase")] Concepto concepto)
        {
            if (ModelState.IsValid)
            {
                db.Concepto.Add(concepto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.id_fase = new SelectList(db.Fase, "id", "Nombre", concepto.id_fase);
            return View(concepto);
        }

 
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                Concepto concepto = await db.Concepto.FindAsync(id);
                db.Concepto.Remove(concepto);
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
