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
    public class PresupuestoController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: Presupuesto

        public async Task<ActionResult> Index()
        {
            var list = from ppto in db.Ppto
                       join pptoEsc in db.EscenarioPpto on ppto.id equals pptoEsc.id_presupuesto into tbl1
                       from pptoEsc in tbl1.DefaultIfEmpty()
                       join esc in db.Escenario on pptoEsc.id_escenario equals esc.id into tbl2
                       from esc in tbl2.DefaultIfEmpty()
                       select new PptoListViewModel
                       {
                           EscenarioPptoId = (pptoEsc == null ? 0 : pptoEsc.id),
                           id = ppto.id,
                           Nombre = ppto.Nombre,
                           AInicial = ppto.AInicial,
                           AFinal = ppto.AFinal,
                           EscenarioNombre = (esc == null ? String.Empty : esc.Nombre )

                       };

            //var list = await db.EscenarioPpto.

            //    Select(x => new PptoListViewModel
            //    {
            //        EscenarioPptoId = x.id,
            //        id = x.Ppto.id,
            //        Nombre = x.Ppto.Nombre,
            //        AInicial = x.Ppto.AInicial,
            //        AFinal = x.Ppto.AFinal,
            //        EscenarioNombre = x.Escenario.Nombre

            //    }).ToListAsync();
            return View(list);
        }

        public async Task<ActionResult> Create()
        {
            var pptoVM = new PptoViewModel();
            pptoVM.Escenario = await db.Escenario.ToListAsync();
            pptoVM.Ppto = await db.Ppto.ToListAsync();
            return View(pptoVM);
        }


        // POST: Presupuesto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PptoViewModel PptoVM)
        {





            if (ModelState.IsValid)
            {

                using (var dbTransact = db.Database.BeginTransaction())
                {
                    var ppto = new Ppto();
                    ppto.Nombre = PptoVM.Nombre;
                    ppto.AInicial = PptoVM.AInicial;
                    ppto.AFinal = PptoVM.AFinal;

                    db.Ppto.Add(ppto);
                    await db.SaveChangesAsync();

                    var escenarioPpto = new EscenarioPpto();
                    escenarioPpto.id_escenario = PptoVM.EscenarioId;
                    escenarioPpto.id_presupuesto = ppto.id;

                    db.EscenarioPpto.Add(escenarioPpto);
                    await db.SaveChangesAsync();

                    dbTransact.Commit();
                }

                return RedirectToAction("Index");
            }

            return View(PptoVM);
        }

        [HttpPost]
        public async Task<JsonResult> CreatePpto(PptoViewModel PptoVM)
        {

            try
            {
                var data = await db.Ppto.Select(x => x.Nombre.ToLower().Replace(" ", "").Trim()).ToListAsync();

                if (data.Contains(PptoVM.Nombre.ToLower().Replace(" ", "").Trim()))
                {
                    return Json(new
                    {
                        errorCode = 1,
                        errorMessage = $"El nombre '{PptoVM.Nombre}' ya existe en el sistema."
                    },
                 JsonRequestBehavior.AllowGet);
                }

                var ppto = new Ppto();
                ppto.Nombre = PptoVM.Nombre;
                ppto.AInicial = PptoVM.AInicial;
                ppto.AFinal = PptoVM.AFinal;

                db.Ppto.Add(ppto);
                await db.SaveChangesAsync();

                return Json(new
                {
                    errorCode = 0,
                    errorMessage = ""
                },
                  JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    errorCode = 10,
                    errorMessage = e.Message
                },
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SavePptoEscenario(PptoViewModel PptoVM)
        {

            try
            {
                var data = await db.EscenarioPpto.Where(x => (x.id_presupuesto == PptoVM.id)).Select(x => x.id_escenario).ToListAsync();

                if (data.Contains(PptoVM.EscenarioId))
                {
                    return Json(new
                    {
                        errorCode = 1,
                        errorMessage = $"Ya existe un registro con los datos seleccionados."
                    },
                 JsonRequestBehavior.AllowGet);
                }

                var escenarioPpto = new EscenarioPpto();
                escenarioPpto.id_escenario = PptoVM.EscenarioId;
                escenarioPpto.id_presupuesto = PptoVM.id;

                db.EscenarioPpto.Add(escenarioPpto);
                await db.SaveChangesAsync();

                return Json(new
                {
                    errorCode = 0,
                    errorMessage = ""
                },
                  JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    errorCode = 10,
                    errorMessage = e.Message
                },
                    JsonRequestBehavior.AllowGet);
            }




 

        }




        // POST: Presupuesto/Delete/5
        public async Task<ActionResult> Delete(long id,long pptoId)
        {

            try
            {


                using (var dbTransact = db.Database.BeginTransaction())
                {

                    var escenarioPpto = db.EscenarioPpto.Where(x => x.id == id).FirstOrDefault();
                    if(escenarioPpto != null)
                    {
                        db.EscenarioPpto.Remove(escenarioPpto);
                        await db.SaveChangesAsync();
                    }
                    
                    var escenarioPptoExist = db.EscenarioPpto.Where(x => x.id_presupuesto == pptoId).FirstOrDefault();

                    if(escenarioPptoExist == null)
                    {
                        var ppto = await db.Ppto.FindAsync(pptoId);
                        db.Ppto.Remove(ppto);
                        await db.SaveChangesAsync();
                    }
                    

                    dbTransact.Commit();
                }



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
