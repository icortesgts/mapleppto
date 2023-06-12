using MaplePPTO.Models;
using MaplePPTO.Models.Dapper;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MaplePPTO.Controllers
{
    public class CompartidoController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();
        private readonly ICargaMasivaDA percistence;
        public CompartidoController()
        {
            percistence = new CargaMasivaDA();
        }
        public async Task<JsonResult> FilterClientByName(String text)
        {

            var list = await db.Cliente.Where(x => x.Nombre.Contains(text)).Select(x => new  { Id = x.id, Nombre = x.Nombre }).Take(15).ToListAsync();

            return Json(new { list = list }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> FilterConcetByName(String text)
        {

            var list = await db.Concepto.Where(x => x.Nombre.Contains(text)).Select(x => new { Id = x.id, Nombre = x.Nombre }).Take(15).ToListAsync();

            return Json(new { list = list }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> FilterCityByName(String text)
        {

            var list = await db.Localizacion.Where(x => x.NomMun.Contains(text)).Select(x => new { Id = x.id, Nombre = x.NomMun }).Take(15).ToListAsync();

            return Json(new { list = list }, JsonRequestBehavior.AllowGet);
        }
        
        public async Task<JsonResult> CalculateBase(long PresupuestoId, long EscenarioId)
        {
            try
            {
                var response = await percistence.ExecCalculateBase(PresupuestoId, EscenarioId);
                return Json(new { message = "success",errorCode = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { message = e.Message, errorCode = 10 }, JsonRequestBehavior.AllowGet);

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