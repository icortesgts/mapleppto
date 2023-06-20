using MaplePPTO.Models;
using MaplePPTO.Models.Dapper;
using MaplePPTO.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MaplePPTO.Controllers
{
    public class FinancieroController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();
        private readonly ICargaMasivaDA percistence;
        public FinancieroController()
        {
            percistence = new CargaMasivaDA();
        }
        // GET: Resultados
        public async Task<ActionResult> Index(ParametroViewModel model)
        {
            if (model == null || !model.PresupuestoId.HasValue)
            {
                model = new ParametroViewModel();
                model.PresupuestoId = new long?(1L);
                model.EscenarioId = new long?(1L);
                model.TipoReporteId = 16;
                model.showMonths = 12;
            }
            GenerateHeaderFinanciero(model);

            //var response = await percistence.GetFinancialReports(model);
            //model.ListReports = response.reports; //GroupReportsByClient(response.reports.GroupBy(x => x.Cliente));

            //if (response.reports != null && response.reports.Count > 0)
            //    GenerateHeader(model, response.reports);

            model.selectLisReportTypes = await db.TipoReporte.Where(x => (x.esFinanciero != null && x.esFinanciero == true && !x.descripcion.Contains("EPOC"))).Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToListAsync();

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

        private List<ResultPpto> GroupReportsByClient(IEnumerable<IGrouping<string, ResultPpto>> enumerable)
        {

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> Index2(ParametroViewModel model)
        {




            model.selectLisReportTypes = await db.TipoReporte.Where(x => (x.esFinanciero != null && x.esFinanciero == true && !x.descripcion.Contains("EPOC"))).Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToListAsync();

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

            if (model.TipoReporteId == 38)
            {
                var result = await percistence.GetSummaryReports();
                model.resultSummaries = result.resultSummaries;
                return View("Summary", model);
            }

            var response = await percistence.GetFinancialReports(model);

            if (response.reports != null && response.reports.Count > 0)
            {
                GenerateHeader(model, response.reports);
                var sahosReports = new  int[]{19,20,23,24 };
                if(sahosReports.ToList().Contains(model.TipoReporteId))
                {
                    model.ListReports = response.reports.OrderBy(x => x.ciudad).ThenBy(x => x.Cliente).ToList();
                    return View("Rev_Pat_SAHOS",model);
                }
                else
                {
                    model.ListReports = response.reports;
                }
               
            }
                


            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> DetailsFinancialReport(ParametroViewModel model)
        {

            try
            {
                var response = await percistence.GetFinancialReports(model);

   
                var jsonResult = Json(new
                {
                    draw = 1,
                    recordsTotal = response.resultFinanciero.Count,
                    recordsFiltered = response.resultFinanciero.Count,
                    data = response.resultFinanciero
                }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            return Json(null, JsonRequestBehavior.AllowGet);
        }

        private List<string[]> DatatoString(List<ResultPpto> arr, int mesesFaltantesAnioBase)
        {
            var data = arr.Select(x =>
            {
                var item = new List<string>() {
                 x.Ppto,
                x.Escenario,
                x.Cliente,
                x.ciudad,
                 "",
                x.concepto,
                x.Fase};

                if (mesesFaltantesAnioBase == 6) item.Add(Math.Round(double.Parse(x.mesant6), 0).ToString());
                if (mesesFaltantesAnioBase >= 5) item.Add(Math.Round(double.Parse(x.mesant5), 0).ToString());
                if (mesesFaltantesAnioBase >= 4) item.Add(Math.Round(double.Parse(x.mesant4), 0).ToString());
                if (mesesFaltantesAnioBase >= 3) item.Add(Math.Round(double.Parse(x.mesant3), 0).ToString());
                if (mesesFaltantesAnioBase >= 2) item.Add(Math.Round(double.Parse(x.mesant2), 0).ToString());
                if (mesesFaltantesAnioBase >= 1) item.Add(Math.Round(double.Parse(x.mesant1), 0).ToString());

                var meses = new string[] {
                 Math.Round(double.Parse(x.mes1), 0).ToString(),
                 Math.Round(double.Parse(x.mes2), 0).ToString(),
                 Math.Round(double.Parse(x.mes3), 0).ToString(),
                 Math.Round(double.Parse(x.mes4), 0).ToString(),
                 Math.Round(double.Parse(x.mes5), 0).ToString(),
                 Math.Round(double.Parse(x.mes6), 0).ToString(),
                 Math.Round(double.Parse(x.mes7), 0).ToString(),
                 Math.Round(double.Parse(x.mes8), 0).ToString(),
                 Math.Round(double.Parse(x.mes9), 0).ToString(),
                 Math.Round(double.Parse(x.mes10), 0).ToString(),
                 Math.Round(double.Parse(x.mes11), 0).ToString(),
                 Math.Round(double.Parse(x.mes12), 0).ToString(),
                 Math.Round(double.Parse(x.mes13), 0).ToString(),
                 Math.Round(double.Parse(x.mes14), 0).ToString(),
                 Math.Round(double.Parse(x.mes15), 0).ToString(),
                 Math.Round(double.Parse(x.mes16), 0).ToString(),
                 Math.Round(double.Parse(x.mes17), 0).ToString(),
                 Math.Round(double.Parse(x.mes18), 0).ToString(),
                 Math.Round(double.Parse(x.mes19), 0).ToString(),
                 Math.Round(double.Parse(x.mes20), 0).ToString(),
                 Math.Round(double.Parse(x.mes21), 0).ToString(),
                 Math.Round(double.Parse(x.mes22), 0).ToString(),
                 Math.Round(double.Parse(x.mes23), 0).ToString(),
                 Math.Round(double.Parse(x.mes24), 0).ToString(),
                 Math.Round(double.Parse(x.mes25), 0).ToString(),
                 Math.Round(double.Parse(x.mes26), 0).ToString(),
                 Math.Round(double.Parse(x.mes27), 0).ToString(),
                 Math.Round(double.Parse(x.mes28), 0).ToString(),
                 Math.Round(double.Parse(x.mes29), 0).ToString(),
                 Math.Round(double.Parse(x.mes30), 0).ToString(),
                 Math.Round(double.Parse(x.mes31), 0).ToString(),
                 Math.Round(double.Parse(x.mes32), 0).ToString(),
                 Math.Round(double.Parse(x.mes33), 0).ToString(),
                 Math.Round(double.Parse(x.mes34), 0).ToString(),
                 Math.Round(double.Parse(x.mes35), 0).ToString(),
                 Math.Round(double.Parse(x.mes36), 0).ToString(),
                 Math.Round(double.Parse(x.mes37), 0).ToString(),
                 Math.Round(double.Parse(x.mes38), 0).ToString(),
                 Math.Round(double.Parse(x.mes39), 0).ToString(),
                 Math.Round(double.Parse(x.mes40), 0).ToString(),
                 Math.Round(double.Parse(x.mes41), 0).ToString(),
                 Math.Round(double.Parse(x.mes42), 0).ToString(),
                 Math.Round(double.Parse(x.mes43), 0).ToString(),
                 Math.Round(double.Parse(x.mes44), 0).ToString(),
                 Math.Round(double.Parse(x.mes45), 0).ToString(),
                 Math.Round(double.Parse(x.mes46), 0).ToString(),
                 Math.Round(double.Parse(x.mes47), 0).ToString(),
                 Math.Round(double.Parse(x.mes48), 0).ToString(),
                 Math.Round(double.Parse(x.mes49), 0).ToString(),
                 Math.Round(double.Parse(x.mes50), 0).ToString(),
                 Math.Round(double.Parse(x.mes51), 0).ToString(),
                 Math.Round(double.Parse(x.mes52), 0).ToString(),
                 Math.Round(double.Parse(x.mes53), 0).ToString(),
                 Math.Round(double.Parse(x.mes54), 0).ToString(),
                 Math.Round(double.Parse(x.mes55), 0).ToString(),
                 Math.Round(double.Parse(x.mes56), 0).ToString(),
                 Math.Round(double.Parse(x.mes57), 0).ToString(),
                 Math.Round(double.Parse(x.mes58), 0).ToString(),
                 Math.Round(double.Parse(x.mes59), 0).ToString(),
                 Math.Round(double.Parse(x.mes60), 0).ToString()
            };

                item.AddRange(meses);
                return item.ToArray();
            }).ToList();

            return data;

        }

        private void GenerateHeader(ParametroViewModel model, List<ResultPpto> ListReports)
        {
            var firstData = ListReports.First();
            var mesesFaltantesAnioBase = (12 - firstData.mes) + 1;
            var mesesProyeccion = 60;
            var totalMeses = mesesProyeccion + mesesFaltantesAnioBase;
            var columnsHeader = new List<string>();

            foreach (var x in Enumerable.Range(0, totalMeses)) // se crea desde cero para tomar el mes actual tambien
            {
                if(x >= mesesFaltantesAnioBase)
                {
                    var textHeader = new DateTime(firstData.anio, firstData.mes, 1).AddMonths(x).ToString("MMM-y").Replace(".", "");
                    //var keyColumnBody = GenerarTextoHeader(firstData.anio, new DateTime(firstData.anio, firstData.mes, 1).AddMonths(x), x, mesesFaltantesAnioBase);
                    columnsHeader.Add(textHeader);
                }
  
            }

            model.listHeader = columnsHeader;
            model.mesesFaltantesAnioBase = mesesFaltantesAnioBase;
        }
        private void GenerateHeaderFinanciero(ParametroViewModel model)
        {
            var ppto = db.Ppto.Where(x => x.id == model.PresupuestoId).FirstOrDefault();
            List<string> stringList = new List<string>();
            foreach (int months in Enumerable.Range(0, 60))
            {
                DateTime dateTime;
                if (months > 0 && months % 12 == 0)
                {
                    dateTime = new DateTime(ppto.AInicial, 1, 1);
                    dateTime = dateTime.AddMonths(months - 1);
                    string str = dateTime.ToString("yyyy").Replace(".", "");
                    stringList.Add("Total " + str);
                }
                dateTime = new DateTime(ppto.AInicial, 1, 1);
                dateTime = dateTime.AddMonths(months);
                string str1 = dateTime.ToString("MMM-y").Replace(".", "");
                stringList.Add(str1);
                if (months == 59)
                {
                    dateTime = new DateTime(ppto.AInicial, 1, 1);
                    dateTime = dateTime.AddMonths(months);
                    string str2 = dateTime.ToString("yyyy").Replace(".", "");
                    stringList.Add("Total " + str2);
                }
            }
            model.listHeader = stringList;
        }
    }
}