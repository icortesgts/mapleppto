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
using MaplePPTO.Models.Dapper;

namespace MaplePPTO.Controllers
{
    public class ParametroController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: Parametro
        public async Task<ActionResult> Index()
        {

            var model = new ParametroViewModel();
            model.PresupuestoId = 0;
            model.PresupuestoId = 0;

            model.listClientes = db.Cliente.Select(x => new ClienteParametroViewModel { Id = x.id, Nombre = x.Nombre }).ToList();
            model.listClientes.Add(new ClienteParametroViewModel { Id=1, Nombre="Otros" });

            model.selectedClients = new long?[] { 1, 10, 11, 12, 13, 14 };

            model.ParametroList = await db.Parametro.Take(1000).Select(x => new ParametroListViewModel
            {
                parametroId = x.id,
                parametroNombre = x.Nombre,
                valornum = (x.ValorNum ?? 0) *100,
                valordate = x.ValorDate,
                Cliente = x.Cliente.Nombre ?? "Otros",
            }).GroupBy(x => x.Cliente).ToListAsync();

            model.listConceptParam = await db.Parametro.Take(1000).Select(x =>x.Nombre).Distinct().ToListAsync();

        model.selectListBudgets = await db.Ppto.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.Nombre
            }).ToListAsync();

            model.selectListEscenaries = await db.Escenario.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.Nombre
            }).ToListAsync();

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Index(ParametroViewModel model)
        {

            //var query = db.Parametro.Where(x=> x.id_escenario == model.EscenarioId && x.Escenario.EscenarioPpto.Any(y => y.id_presupuesto == model.PresupuestoId)).Select(x => new ParametroListViewModel
            //{
            //    parametroId = x.id,
            //    parametroNombre = x.Nombre,
            //    valorstr = x.ValorStr,
            //    valornum = x.ValorNum ?? 0,
            //    valordate = x.ValorDate,
            //    //Contrato = x.Contrato.Contrato1,
            //    Cliente = x.Cliente.Nombre,
            //    Fase = x.Fase.Nombre,
            //    Escenario = x.Escenario.Nombre,
            //});

            model.listClientes = db.Cliente.Select(x => new ClienteParametroViewModel { Id = x.id, Nombre = x.Nombre }).ToList();
            model.listClientes.Add(new ClienteParametroViewModel { Id = 1, Nombre = "Otros" });

            //model.selectedlients = new long?[] { null, 10, 11, 12, 13, 14 };

            model.ParametroList = await db.Parametro.Where(x => x.id_escenario == model.EscenarioId && x.Escenario.EscenarioPpto.Any(y => y.id_presupuesto == model.PresupuestoId)).Select(x => new ParametroListViewModel
            {
                parametroId = x.id,
                parametroNombre = x.Nombre,
                valornum = (x.ValorNum ?? 0) * 100,
                valordate = x.ValorDate,
                Cliente = x.Cliente.Nombre ?? "Otros",
            }).GroupBy(x => x.Cliente).ToListAsync();

            model.listConceptParam = await db.Parametro.Select(x => x.Nombre).Distinct().ToListAsync();

            //model.ParametroList = await query.ToListAsync();
            model.selectListBudgets = await db.Ppto.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.Nombre
            }).ToListAsync();

            model.selectListEscenaries = await db.Escenario.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.Nombre
            }).ToListAsync();

            return View(model);
        }

        // GET: Parametro/Create
        public ActionResult Create()
        {
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "Nombre");
            ViewBag.id_escenario = new SelectList(db.Escenario, "id", "Nombre");
            ViewBag.id_fase = new SelectList(db.Fase, "id", "Nombre");
            return View();
        }

        // POST: Parametro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Parametro.Add(parametro);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "Nombre", parametro.id_cliente);
            ViewBag.id_escenario = new SelectList(db.Escenario, "id", "Nombre", parametro.id_escenario);
            ViewBag.id_fase = new SelectList(db.Fase, "id", "Nombre", parametro.id_fase);
            return View(parametro);
        }

        [HttpPost]
        public async Task<ActionResult> BulkCreateOrUpdate(List<Parametro> parametros)
        {
            var response = new RespuestaDapperViewModel();
            if (!ModelState.IsValid)
            {
                //response.errorCode = 10;
                //response.merror = "An error ocurred!. validate the data";
                //return Json(response, JsonRequestBehavior.AllowGet);

            }
            try
            {
                var datosSesion = Helper.SesionModel.DatosSesion;
                ICargaMasivaDA percistence = new CargaMasivaDA();
                response = await percistence.BulkInsertOrUpdateParametro(parametros, datosSesion.id_usuario);

            }
            catch (Exception e)
            {
                response.errorCode = 10;
                response.merror = e.Message;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: Test/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametro parametro = await db.Parametro.FindAsync(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "Nombre", parametro.id_cliente);
            ViewBag.id_escenario = new SelectList(db.Escenario, "id", "Nombre", parametro.id_escenario);
            ViewBag.id_fase = new SelectList(db.Fase, "id", "Nombre", parametro.id_fase);
            //ViewBag.id_ppto = new SelectList(db.Ppto, "id", "Nombre", parametro.id_ppto);
            return View(parametro);
        }

        // POST: Test/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Nombre,ValorStr,ValorNum,ValorDate,fecha_crea,fecha_modifica,usuario_crea,usuario_modifica,id_fase,id_cliente,id_escenario,id_ppto")] Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parametro).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "Nombre", parametro.id_cliente);
            ViewBag.id_escenario = new SelectList(db.Escenario, "id", "Nombre", parametro.id_escenario);
            ViewBag.id_fase = new SelectList(db.Fase, "id", "Nombre", parametro.id_fase);
            //ViewBag.id_ppto = new SelectList(db.Ppto, "id", "Nombre", parametro.id_ppto);
            return View(parametro);
        }


        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                Parametro parametro = await db.Parametro.FindAsync(id);
                db.Parametro.Remove(parametro);
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
