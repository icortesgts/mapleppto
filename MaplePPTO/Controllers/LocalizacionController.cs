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
using MaplePPTO.ViewModels;

namespace MaplePPTO.Controllers
{
    public class LocalizacionController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: Localizacions
        public async Task<ActionResult> Index()
        {
            var localizacion = db.Localizacion.Include(l => l.Pais);
            return View(await localizacion.ToListAsync());
        }

        // GET: Localizacions/Create
        public async Task<ActionResult> Create()
        {
            var model = new LocalizacionViewModel();

            model.selectListPaices = await db.Pais.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nombre
            }).ToListAsync();

            model.selectListDepartamentos = await db.Localizacion.GroupBy(x => x.NomDpto).Select(x => x.FirstOrDefault())
                .Select(x => new SelectListItem
            {
                Value = x.NomDpto,
                Text = x.NomDpto
            }).ToListAsync();
       
            return View(model);
        }

        // POST: Localizacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LocalizacionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var localizacion = new Localizacion();

                db.Localizacion.Add(localizacion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            model.selectListPaices = await db.Pais.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nombre
            }).ToListAsync();

            model.selectListDepartamentos = await db.Localizacion.GroupBy(x => x.NomDpto).Select(x => x.First())
                .Select(x => new SelectListItem
                {
                    Value = x.NomDpto,
                    Text = x.NomDpto
                }).ToListAsync();

            return View(model);
        }



        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                Localizacion localizacion = await db.Localizacion.FindAsync(id);
                db.Localizacion.Remove(localizacion);
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
