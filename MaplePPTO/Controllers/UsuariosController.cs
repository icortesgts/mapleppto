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
    public class UsuariosController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            var usuarios = db.Usuarios.Include(u => u.Perfil);
            return View(await usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = await db.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.id_perfil = new SelectList(db.Perfil, "id", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "login,Password,Nombre,Email,Bloqueado,Fecha_Password,Fecha_Creacion,Fecha_Modificacion,id_perfil,Id,Token,fecha_crea,fecha_modifica,usuario_crea,usuario_modifica")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuarios);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.id_perfil = new SelectList(db.Perfil, "id", "Nombre", usuarios.id_perfil);
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = await db.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_perfil = new SelectList(db.Perfil, "id", "Nombre", usuarios.id_perfil);
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "login,Password,Nombre,Email,Bloqueado,Fecha_Password,Fecha_Creacion,Fecha_Modificacion,id_perfil,Id,Token,fecha_crea,fecha_modifica,usuario_crea,usuario_modifica")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.id_perfil = new SelectList(db.Perfil, "id", "Nombre", usuarios.id_perfil);
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = await db.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Usuarios usuarios = await db.Usuarios.FindAsync(id);
            db.Usuarios.Remove(usuarios);
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
