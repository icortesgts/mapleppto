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
    public class PerfilesController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: Perfiles
        public async Task<ActionResult> Index()
        {
            return View(await db.Perfil.ToListAsync());
        }

        // GET: Perfiles/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = await db.Perfil.FindAsync(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        // GET: Perfiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Perfiles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Nombre,Descripcion,fecha_crea,fecha_modifica,usuario_crea,usuario_modifica")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.Perfil.Add(perfil);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(perfil);
        }

        // GET: Perfiles/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = await db.Perfil.FindAsync(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        // POST: Perfiles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Nombre,Descripcion,fecha_crea,fecha_modifica,usuario_crea,usuario_modifica")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perfil).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(perfil);
        }

        // GET: Perfiles/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = await db.Perfil.FindAsync(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        // POST: Perfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Perfil perfil = await db.Perfil.FindAsync(id);
            db.Perfil.Remove(perfil);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
