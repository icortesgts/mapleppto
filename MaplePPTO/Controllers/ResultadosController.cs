using MaplePPTO.Models;
using MaplePPTO.Models.Dapper;
using MaplePPTO.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MaplePPTO.Controllers
{
    public class ResultadosController : Controller
    {
        private MAPLEEntities db = new MAPLEEntities();
        private readonly ICargaMasivaDA percistence;
        public ResultadosController()
        {
            percistence = new CargaMasivaDA();
        }
        // GET: Resultados
        public async Task<ActionResult> Index()
        {
            var model = new ParametroViewModel();
            model.PresupuestoId = 1;
            model.EscenarioId = 1;
            model.TipoReporteId = 1;
            model.showMonths = 12;

            try
            {
                var response = await percistence.GetReports(model);
                //model.ListReports = response.reports;//.OrderBy(x=>x.ciudad).ThenBy(x=>x.Cliente).ThenBy(x=>x.anio).ThenBy(x=>x.mes).ToList();


                if (response.reports != null && response.reports.Count > 0) //model.ListReports.Count > 0)
                {
                    GenerateHeader(model, response.reports);
                    //model.ListReports = new List<ResultPpto>();
                    //model.ListReports.Add(new ResultPpto() { Escenario = "loading" });

                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            model.selectLisReportTypes = await db.TipoReporte.Where(x => x.esFinanciero != null && x.esFinanciero == false).Select(x => new SelectListItem
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
        [HttpPost]
        public async Task<ActionResult> Index(ParametroViewModel model)
        {
            var response = await percistence.GetReports(model);
            //model.ListReports = response.reports;

            if (response.reports != null && response.reports.Count > 0) //model.ListReports.Count > 0)
                GenerateHeader(model, response.reports);

            model.selectLisReportTypes = await db.TipoReporte.Where(x => x.esFinanciero != null && x.esFinanciero == false).Select(x => new SelectListItem
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

        [HttpPost]
        public async Task<JsonResult> DetailsOperationReport(ParametroViewModel model)
        {


            //var model = new ParametroViewModel();
            //model.PresupuestoId = 1;
            //model.EscenarioId = 1;
            //model.TipoReporteId = 1;
            //model.showMonths = 12;

            try
            {
                var response = await percistence.GetReports(model);

                var data = DatatoString(response.reports, model.mesesFaltantesAnioBase);

                var jsonResult = Json(new
                {
                    draw = 1,
                    recordsTotal = data.Count,
                    recordsFiltered = data.Count,
                    data
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

        [HttpPost]
        public async Task<JsonResult> SummaryOperationReport(ParametroViewModel model)
        {

            if (model.summaryReporType == 0)
            {
                var empty = new string[] { };
                return Json(new
                {
                    draw = 1,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = empty
                },
                    JsonRequestBehavior.AllowGet);
            }
            var response = await percistence.GetReports(model);
            switch (model.summaryReporType)
            {
                case 1:
                    var data = response.reports.GroupBy(x => x.concepto).Select(x => SumDatatoString(x.ToList(), model.mesesFaltantesAnioBase)).ToList();


                    var jsonResult = Json(new
                    {
                        draw = 1,
                        recordsTotal = data.Count,
                        recordsFiltered = data.Count,
                        data
                    }, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                case 2:
                    var arrayInput2 = response.reports.GroupBy(x => x.ciudad).Select(x => x.ToList());
                    var data2 = new List<string[]>();
                    foreach (var arr in arrayInput2)
                    {
                        var range = arr.GroupBy(x => x.concepto).Select(x => SumDatatoString(x.ToList(), model.mesesFaltantesAnioBase)).ToList();
                        data2.AddRange(range);
                    }

                    var jsonResult2 = Json(new
                    {
                        draw = 1,
                        recordsTotal = data2.Count,
                        recordsFiltered = data2.Count,
                        data = data2
                    }, JsonRequestBehavior.AllowGet);
                    jsonResult2.MaxJsonLength = int.MaxValue;
                    return jsonResult2;
                case 3:
                    var arrayInput3 = response.reports.GroupBy(x => x.Cliente).Select(x => x.ToList());
                    var data3 = new List<string[]>();
                    foreach (var arr in arrayInput3)
                    {
                        var range = arr.GroupBy(x => x.concepto).Select(x => SumDatatoString(x.ToList(), model.mesesFaltantesAnioBase)).ToList();
                        data3.AddRange(range);
                    }
                    var jsonResult3 = Json(new
                    {
                        draw = 1,
                        recordsTotal = data3.Count,
                        recordsFiltered = data3.Count,
                        data = data3
                    }, JsonRequestBehavior.AllowGet);
                    jsonResult3.MaxJsonLength = int.MaxValue;
                    return jsonResult3;
            }


            return Json(null, JsonRequestBehavior.AllowGet);
        }
        private ResultPpto SumData(List<ResultPpto> arr)
        {
            //return  await Task.Run<ResultPpto>(()=> {
            return new ResultPpto()
            {
                Ppto = arr[0].Ppto,
                Escenario = arr[0].Escenario,
                Cliente = arr[0].Cliente,
                ciudad = arr[0].ciudad,
                Contrato = "",
                concepto = arr[0].concepto,
                Fase = arr[0].Fase,
                mesant6 = arr.Sum(x => Math.Round(double.Parse(x.mesant6), 0)).ToString(),
                mesant5 = arr.Sum(x => Math.Round(double.Parse(x.mesant5), 0)).ToString(),
                mesant4 = arr.Sum(x => Math.Round(double.Parse(x.mesant4), 0)).ToString(),
                mesant3 = arr.Sum(x => Math.Round(double.Parse(x.mesant3), 0)).ToString(),
                mesant2 = arr.Sum(x => Math.Round(double.Parse(x.mesant2), 0)).ToString(),
                mesant1 = arr.Sum(x => Math.Round(double.Parse(x.mesant1), 0)).ToString(),

                mes1 = arr.Sum(x => Math.Round(double.Parse(x.mes1), 0)).ToString(),
                mes2 = arr.Sum(x => Math.Round(double.Parse(x.mes2), 0)).ToString(),
                mes3 = arr.Sum(x => Math.Round(double.Parse(x.mes3), 0)).ToString(),
                mes4 = arr.Sum(x => Math.Round(double.Parse(x.mes4), 0)).ToString(),
                mes5 = arr.Sum(x => Math.Round(double.Parse(x.mes5), 0)).ToString(),
                mes6 = arr.Sum(x => Math.Round(double.Parse(x.mes6), 0)).ToString(),
                mes7 = arr.Sum(x => Math.Round(double.Parse(x.mes7), 0)).ToString(),
                mes8 = arr.Sum(x => Math.Round(double.Parse(x.mes8), 0)).ToString(),
                mes9 = arr.Sum(x => Math.Round(double.Parse(x.mes9), 0)).ToString(),
                mes10 = arr.Sum(x => Math.Round(double.Parse(x.mes10), 0)).ToString(),
                mes11 = arr.Sum(x => Math.Round(double.Parse(x.mes11), 0)).ToString(),
                mes12 = arr.Sum(x => Math.Round(double.Parse(x.mes12), 0)).ToString(),
                mes13 = arr.Sum(x => Math.Round(double.Parse(x.mes13), 0)).ToString(),
                mes14 = arr.Sum(x => Math.Round(double.Parse(x.mes14), 0)).ToString(),
                mes15 = arr.Sum(x => Math.Round(double.Parse(x.mes15), 0)).ToString(),
                mes16 = arr.Sum(x => Math.Round(double.Parse(x.mes16), 0)).ToString(),
                mes17 = arr.Sum(x => Math.Round(double.Parse(x.mes17), 0)).ToString(),
                mes18 = arr.Sum(x => Math.Round(double.Parse(x.mes18), 0)).ToString(),
                mes19 = arr.Sum(x => Math.Round(double.Parse(x.mes19), 0)).ToString(),
                mes20 = arr.Sum(x => Math.Round(double.Parse(x.mes20), 0)).ToString(),
                mes21 = arr.Sum(x => Math.Round(double.Parse(x.mes21), 0)).ToString(),
                mes22 = arr.Sum(x => Math.Round(double.Parse(x.mes22), 0)).ToString(),
                mes23 = arr.Sum(x => Math.Round(double.Parse(x.mes23), 0)).ToString(),
                mes24 = arr.Sum(x => Math.Round(double.Parse(x.mes24), 0)).ToString(),
                mes25 = arr.Sum(x => Math.Round(double.Parse(x.mes25), 0)).ToString(),
                mes26 = arr.Sum(x => Math.Round(double.Parse(x.mes26), 0)).ToString(),
                mes27 = arr.Sum(x => Math.Round(double.Parse(x.mes27), 0)).ToString(),
                mes28 = arr.Sum(x => Math.Round(double.Parse(x.mes28), 0)).ToString(),
                mes29 = arr.Sum(x => Math.Round(double.Parse(x.mes29), 0)).ToString(),
                mes30 = arr.Sum(x => Math.Round(double.Parse(x.mes30), 0)).ToString(),
                mes31 = arr.Sum(x => Math.Round(double.Parse(x.mes31), 0)).ToString(),
                mes32 = arr.Sum(x => Math.Round(double.Parse(x.mes32), 0)).ToString(),
                mes33 = arr.Sum(x => Math.Round(double.Parse(x.mes33), 0)).ToString(),
                mes34 = arr.Sum(x => Math.Round(double.Parse(x.mes34), 0)).ToString(),
                mes35 = arr.Sum(x => Math.Round(double.Parse(x.mes35), 0)).ToString(),
                mes36 = arr.Sum(x => Math.Round(double.Parse(x.mes36), 0)).ToString(),
                mes37 = arr.Sum(x => Math.Round(double.Parse(x.mes37), 0)).ToString(),
                mes38 = arr.Sum(x => Math.Round(double.Parse(x.mes38), 0)).ToString(),
                mes39 = arr.Sum(x => Math.Round(double.Parse(x.mes39), 0)).ToString(),
                mes40 = arr.Sum(x => Math.Round(double.Parse(x.mes40), 0)).ToString(),
                mes41 = arr.Sum(x => Math.Round(double.Parse(x.mes41), 0)).ToString(),
                mes42 = arr.Sum(x => Math.Round(double.Parse(x.mes42), 0)).ToString(),
                mes43 = arr.Sum(x => Math.Round(double.Parse(x.mes43), 0)).ToString(),
                mes44 = arr.Sum(x => Math.Round(double.Parse(x.mes44), 0)).ToString(),
                mes45 = arr.Sum(x => Math.Round(double.Parse(x.mes45), 0)).ToString(),
                mes46 = arr.Sum(x => Math.Round(double.Parse(x.mes46), 0)).ToString(),
                mes47 = arr.Sum(x => Math.Round(double.Parse(x.mes47), 0)).ToString(),
                mes48 = arr.Sum(x => Math.Round(double.Parse(x.mes48), 0)).ToString(),
                mes49 = arr.Sum(x => Math.Round(double.Parse(x.mes49), 0)).ToString(),
                mes50 = arr.Sum(x => Math.Round(double.Parse(x.mes50), 0)).ToString(),
                mes51 = arr.Sum(x => Math.Round(double.Parse(x.mes51), 0)).ToString(),
                mes52 = arr.Sum(x => Math.Round(double.Parse(x.mes52), 0)).ToString(),
                mes53 = arr.Sum(x => Math.Round(double.Parse(x.mes53), 0)).ToString(),
                mes54 = arr.Sum(x => Math.Round(double.Parse(x.mes54), 0)).ToString(),
                mes55 = arr.Sum(x => Math.Round(double.Parse(x.mes55), 0)).ToString(),
                mes56 = arr.Sum(x => Math.Round(double.Parse(x.mes56), 0)).ToString(),
                mes57 = arr.Sum(x => Math.Round(double.Parse(x.mes57), 0)).ToString(),
                mes58 = arr.Sum(x => Math.Round(double.Parse(x.mes58), 0)).ToString(),
                mes59 = arr.Sum(x => Math.Round(double.Parse(x.mes59), 0)).ToString(),
                mes60 = arr.Sum(x => Math.Round(double.Parse(x.mes60), 0)).ToString()
            };
            //});

        }



        private string[] SumDatatoString(List<ResultPpto> arr, int mesesFaltantesAnioBase)
        {
            //return  await Task.Run<ResultPpto>(()=> {
            var data = new List<string>() {
                 arr[0].Ppto,
                 arr[0].Escenario,
                 arr[0].Cliente,
                 arr[0].ciudad,
                 "",
                 arr[0].concepto,
                 arr[0].Fase};



            if (mesesFaltantesAnioBase == 6)
            {
                data.Add(arr.Sum(x => Math.Round(double.Parse(x.mesant6), 0)).ToString());
            }
            if (mesesFaltantesAnioBase >= 5)
            {
                data.Add(arr.Sum(x => Math.Round(double.Parse(x.mesant5), 0)).ToString());
            }
            if (mesesFaltantesAnioBase >= 4)
            {
                data.Add(arr.Sum(x => Math.Round(double.Parse(x.mesant4), 0)).ToString());
            }
            if (mesesFaltantesAnioBase >= 3)
            {
                data.Add(arr.Sum(x => Math.Round(double.Parse(x.mesant3), 0)).ToString());
            }
            if (mesesFaltantesAnioBase >= 2)
            {
                data.Add(arr.Sum(x => Math.Round(double.Parse(x.mesant2), 0)).ToString());
            }
            if (mesesFaltantesAnioBase >= 1)
            {
                data.Add(arr.Sum(x => Math.Round(double.Parse(x.mesant1), 0)).ToString());
            }





            var meses = new string[] {
                arr.Sum(x => Math.Round(double.Parse(x.mes1), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes2), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes3), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes4), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes5), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes6), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes7), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes8), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes9), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes10), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes11), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes12), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes13), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes14), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes15), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes16), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes17), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes18), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes19), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes20), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes21), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes22), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes23), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes24), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes25), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes26), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes27), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes28), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes29), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes30), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes31), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes32), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes33), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes34), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes35), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes36), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes37), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes38), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes39), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes40), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes41), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes42), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes43), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes44), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes45), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes46), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes47), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes48), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes49), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes50), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes51), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes52), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes53), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes54), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes55), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes56), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes57), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes58), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes59), 0)).ToString(),
                 arr.Sum(x => Math.Round(double.Parse(x.mes60), 0)).ToString()
            };

            data.AddRange(meses);
            return data.ToArray();
            //});

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
                var textHeader = new DateTime(firstData.anio, firstData.mes, 1).AddMonths(x).ToString("MMM-y").Replace(".", "");
                //var keyColumnBody = GenerarTextoHeader(firstData.anio, new DateTime(firstData.anio, firstData.mes, 1).AddMonths(x), x, mesesFaltantesAnioBase);
                columnsHeader.Add(textHeader);
            }

            model.listHeader = columnsHeader;
            model.mesesFaltantesAnioBase = mesesFaltantesAnioBase;
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
        private string GenerarTextoHeader(int anioBase, DateTime fechaLoop, int index, int mesesFaltantes)
        {
            var anioDif = fechaLoop.Year - anioBase;
            return anioDif == 0 ? $"mesant{index++}" : $"mes{(index++) - mesesFaltantes}";

        }

        public virtual async Task<ActionResult> DownloadExcel(long PresupuestoId,long EscenarioId,int TipoReporteId,int showMonths)
        {
            var codes = new string[] { "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                                        "AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ",
                                        "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU" };
            var codeindex = 7;

            var model = new ParametroViewModel();
            model.PresupuestoId = PresupuestoId;
            model.EscenarioId = EscenarioId;
            model.TipoReporteId = TipoReporteId;
            model.showMonths = showMonths;
            var response = await percistence.GetReports(model);

            if (response.reports != null && response.reports.Count > 0) //model.ListReports.Count > 0)
            {
                GenerateHeader(model, response.reports);
                model.ListReports = response.reports;
                //model.ListReports.Add(new ResultPpto() { Escenario = "loading" });

            }

            var Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "PRESUPUESTO";
            Sheet.Cells["B1"].Value = "ESCENARIO";
            Sheet.Cells["C1"].Value = "CLIENTE";
            Sheet.Cells["D1"].Value = "CIUDAD";
            Sheet.Cells["E1"].Value = "CONTRATO";
            Sheet.Cells["F1"].Value = "CONCEPTO";
            Sheet.Cells["G1"].Value = "FASE";
            foreach (var item in model.listHeader)
            {
                Sheet.Cells[$"{codes[codeindex]}1"].Value = item;
                codeindex++;
            }
            int row = 2;
            foreach (var item in model.ListReports)
            {

                

                Sheet.Cells[string.Format("A{0}", row)].Value = item.Ppto;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Escenario;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Cliente;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.ciudad;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.Contrato;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.concepto;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.Fase;

                var columnindex = 7;
                if (model.mesesFaltantesAnioBase == 6)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant6), 0);
                }
                if (model.mesesFaltantesAnioBase >= 5)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant5), 0);
                }
                if (model.mesesFaltantesAnioBase >= 4)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant4), 0);
                }
                if (model.mesesFaltantesAnioBase >= 3)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant3), 0);
                }
                if (model.mesesFaltantesAnioBase >= 2)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant2), 0);
                }
                if (model.mesesFaltantesAnioBase >= 1)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant1), 0);
                }
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes1), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes2), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes3), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes4), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes5), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes6), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes7), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes8), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes9), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes10), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes11), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes12), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes13), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes14), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes15), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes16), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes17), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes18), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes19), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes20), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes21), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes22), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes23), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes24), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes25), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes26), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes27), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes28), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes29), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes30), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes31), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes32), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes33), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes34), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes35), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes36), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes37), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes38), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes39), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes40), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes41), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes42), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes43), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes44), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes45), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes46), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes47), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes48), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes49), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes50), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes51), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes52), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes53), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes54), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes55), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes56), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes57), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes58), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes59), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes60), 0);
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

            return View();
        }

        public virtual async Task<ActionResult> DownloadExcelSummary(long PresupuestoId, long EscenarioId, int TipoReporteId, int showMonths)
        {
            var codes = new string[] { "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                                        "AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ",
                                        "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU" };
            var codeindex = 7;

            var model = new ParametroViewModel();
            model.PresupuestoId = PresupuestoId;
            model.EscenarioId = EscenarioId;
            model.TipoReporteId = TipoReporteId;
            model.showMonths = showMonths;
            var response = await percistence.GetReports(model);

            if (response.reports != null && response.reports.Count > 0) //model.ListReports.Count > 0)
            {
                GenerateHeader(model, response.reports);
                model.ListReports = response.reports;
                //model.ListReports.Add(new ResultPpto() { Escenario = "loading" });

            }

            var Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "PRESUPUESTO";
            Sheet.Cells["B1"].Value = "ESCENARIO";
            Sheet.Cells["C1"].Value = "CLIENTE";
            Sheet.Cells["D1"].Value = "CIUDAD";
            Sheet.Cells["E1"].Value = "CONTRATO";
            Sheet.Cells["F1"].Value = "CONCEPTO";
            Sheet.Cells["G1"].Value = "FASE";
            foreach (var item in model.listHeader)
            {
                Sheet.Cells[$"{codes[codeindex]}1"].Value = item;
                codeindex++;
            }
            int row = 2;
            foreach (var item in model.ListReports)
            {



                Sheet.Cells[string.Format("A{0}", row)].Value = item.Ppto;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Escenario;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Cliente;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.ciudad;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.Contrato;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.concepto;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.Fase;

                var columnindex = 7;
                if (model.mesesFaltantesAnioBase == 6)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant6), 0);
                }
                if (model.mesesFaltantesAnioBase >= 5)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant5), 0);
                }
                if (model.mesesFaltantesAnioBase >= 4)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant4), 0);
                }
                if (model.mesesFaltantesAnioBase >= 3)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant3), 0);
                }
                if (model.mesesFaltantesAnioBase >= 2)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant2), 0);
                }
                if (model.mesesFaltantesAnioBase >= 1)
                {
                    Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mesant1), 0);
                }
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes1), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes2), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes3), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes4), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes5), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes6), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes7), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes8), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes9), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes10), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes11), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes12), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes13), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes14), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes15), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes16), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes17), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes18), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes19), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes20), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes21), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes22), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes23), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes24), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes25), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes26), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes27), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes28), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes29), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes30), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes31), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes32), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes33), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes34), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes35), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes36), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes37), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes38), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes39), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes40), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes41), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes42), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes43), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes44), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes45), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes46), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes47), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes48), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes49), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes50), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes51), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes52), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes53), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes54), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes55), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes56), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes57), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes58), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes59), 0);
                Sheet.Cells[$"{codes[columnindex++]}{row}"].Value = Math.Round(double.Parse(item.mes60), 0);
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

            return View();
        }
        private DataTable GetTableTypeResultPpto(List<ResultPpto> Data)
        {
            DataTable tabla = new DataTable();

            tabla.Columns.Add("mesant6");
            tabla.Columns.Add("mesant5");
            tabla.Columns.Add("mesant4");
            tabla.Columns.Add("mesant3");
            tabla.Columns.Add("mesant2");
            tabla.Columns.Add("mesant1");
            tabla.Columns.Add("mes1");
            tabla.Columns.Add("mes2");
            tabla.Columns.Add("mes3");
            tabla.Columns.Add("mes4");
            tabla.Columns.Add("mes5");
            tabla.Columns.Add("mes6");
            tabla.Columns.Add("mes7");
            tabla.Columns.Add("mes8");
            tabla.Columns.Add("mes9");
            tabla.Columns.Add("mes10");
            tabla.Columns.Add("mes11");
            tabla.Columns.Add("mes12");
            tabla.Columns.Add("mes13");
            tabla.Columns.Add("mes14");
            tabla.Columns.Add("mes15");
            tabla.Columns.Add("mes16");
            tabla.Columns.Add("mes17");
            tabla.Columns.Add("mes18");
            tabla.Columns.Add("mes19");
            tabla.Columns.Add("mes20");
            tabla.Columns.Add("mes21");
            tabla.Columns.Add("mes22");
            tabla.Columns.Add("mes23");
            tabla.Columns.Add("mes24");
            tabla.Columns.Add("mes25");
            tabla.Columns.Add("mes26");
            tabla.Columns.Add("mes27");
            tabla.Columns.Add("mes28");
            tabla.Columns.Add("mes29");
            tabla.Columns.Add("mes30");
            tabla.Columns.Add("mes31");
            tabla.Columns.Add("mes32");
            tabla.Columns.Add("mes33");
            tabla.Columns.Add("mes34");
            tabla.Columns.Add("mes35");
            tabla.Columns.Add("mes36");
            tabla.Columns.Add("mes37");
            tabla.Columns.Add("mes38");
            tabla.Columns.Add("mes39");
            tabla.Columns.Add("mes40");
            tabla.Columns.Add("mes41");
            tabla.Columns.Add("mes42");
            tabla.Columns.Add("mes43");
            tabla.Columns.Add("mes44");
            tabla.Columns.Add("mes45");
            tabla.Columns.Add("mes46");
            tabla.Columns.Add("mes47");
            tabla.Columns.Add("mes48");
            tabla.Columns.Add("mes49");
            tabla.Columns.Add("mes50");
            tabla.Columns.Add("mes51");
            tabla.Columns.Add("mes52");
            tabla.Columns.Add("mes53");
            tabla.Columns.Add("mes54");
            tabla.Columns.Add("mes55");
            tabla.Columns.Add("mes56");
            tabla.Columns.Add("mes57");
            tabla.Columns.Add("mes58");
            tabla.Columns.Add("mes59");
            tabla.Columns.Add("mes60");

            foreach (var item in Data)
            {
                DataRow dr = tabla.NewRow();
                dr[0] = item.mesant6;
                dr[1] = item.mesant5;
                dr[2] = item.mesant4;
                dr[3] = item.mesant3;
                dr[4] = item.mesant2;
                dr[5] = item.mesant1;
                dr[6] = item.mes1;
                dr[7] = item.mes2;
                dr[8] = item.mes3;
                dr[9] = item.mes4;
                dr[10] = item.mes5;
                dr[11] = item.mes6;
                dr[12] = item.mes7;
                dr[13] = item.mes8;
                dr[14] = item.mes9;
                dr[15] = item.mes10;
                dr[16] = item.mes11;
                dr[17] = item.mes12;
                dr[18] = item.mes13;
                dr[19] = item.mes14;
                dr[20] = item.mes15;
                dr[21] = item.mes16;
                dr[22] = item.mes17;
                dr[23] = item.mes18;
                dr[24] = item.mes19;
                dr[25] = item.mes20;
                dr[26] = item.mes21;
                dr[27] = item.mes22;
                dr[28] = item.mes23;
                dr[29] = item.mes24;
                dr[30] = item.mes25;
                dr[31] = item.mes26;
                dr[32] = item.mes27;
                dr[33] = item.mes28;
                dr[34] = item.mes29;
                dr[35] = item.mes30;
                dr[36] = item.mes31;
                dr[37] = item.mes32;
                dr[38] = item.mes33;
                dr[39] = item.mes34;
                dr[40] = item.mes35;
                dr[41] = item.mes36;
                dr[42] = item.mes37;
                dr[43] = item.mes38;
                dr[44] = item.mes39;
                dr[45] = item.mes40;
                dr[46] = item.mes41;
                dr[47] = item.mes42;
                dr[48] = item.mes43;
                dr[49] = item.mes44;
                dr[50] = item.mes45;
                dr[51] = item.mes46;
                dr[52] = item.mes47;
                dr[53] = item.mes48;
                dr[54] = item.mes49;
                dr[55] = item.mes50;
                dr[56] = item.mes51;
                dr[57] = item.mes52;
                dr[58] = item.mes53;
                dr[59] = item.mes54;
                dr[60] = item.mes55;
                dr[61] = item.mes56;
                dr[62] = item.mes57;
                dr[63] = item.mes58;
                dr[64] = item.mes59;
                dr[65] = item.mes60;

                tabla.Rows.Add(dr);
            }
            return tabla;
        }
    }
}
