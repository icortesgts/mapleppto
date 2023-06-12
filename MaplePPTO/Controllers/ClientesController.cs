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
    public class ClientesController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();

        // GET: Clientes
        public async Task<ActionResult> Index()
        {
            var cliente = db.Cliente.Include(c => c.Localizacion);
            return View(await cliente.ToListAsync());
        }


        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.id_localizacion = new SelectList(db.Localizacion, "id", "NomMun");
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Nombre,tipo_persona,Tipo_ID,Identificacion,DV,Direccion,Telfono,emailR,id_localizacion,Tipo,Prospecto,Bloqueado,fecha_crea,fecha_modifica,usuario_crea,usuario_modifica")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.id_localizacion = new SelectList(db.Localizacion, "id", "coddpto", cliente.id_localizacion);
            return View(cliente);
        }

  

        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                Cliente cliente = await db.Cliente.FindAsync(id);
                db.Cliente.Remove(cliente);
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


        public async Task<ActionResult> GetAdditionalInformation(long clientId)
        {
            var model = new ClientAdditionalInfomartionViewModel();
            model.ContratosList = await db.Contrato.Where(x => x.id_cliente == clientId).ToListAsync();
            return PartialView("_additionalInformation", model);
        }

        public async Task<ActionResult> GetContratoEspecificacion(long contratId)
        {
            var model = new ClientAdditionalInfomartionViewModel();
            model.ContratoLocalizacionList = await db.ContratoLocalizacion.Where(x => x.id_contrato == contratId).ToListAsync();
            model.ContratoFaseList = await db.ContratoFase.Where(x => x.id_contrato == contratId).ToListAsync();
            return PartialView("_contratoEspecificacion", model);
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
