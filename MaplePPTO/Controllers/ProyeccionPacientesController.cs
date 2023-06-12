using ExcelDataReader;
using MaplePPTO.Models;
using MaplePPTO.Models.Dapper;
using MaplePPTO.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MaplePPTO.ViewModels.BulkPacientesViewModel;

namespace MaplePPTO.Controllers
{
    public class ProyeccionPacientesController : Controller
    {

        private MAPLEEntities db = new MAPLEEntities();
        // GET: ProyeccionPacientes

        public async Task<ActionResult> Index()
        {
            //var list = await db.ConceptoPpto.Select(x => x).ToListAsync();
            var model = new ConceptoPptoViewModel();
            model.listConcetoPptoVM = await db.ConceptoPpto.Select(x=> new ConceptoPptoViewModel
            {
                ConceptoPptoId = x.id,
                Escenario = x.Escenario.Nombre,
                Presupuesto = x.Ppto.Nombre,
                Ciudad =x.Localizacion.NomDpto,
                Concepto = x.Concepto.Nombre,
                Fase =x.Fase.Nombre,
                Cliente =x.Cliente.Nombre,
                Contrato = x.Contrato.Contrato1,
                Anio = x.Vigencia,
                Mes = x.Mes,
                ValorPeriodo = x.valor

            }).Take(200).ToListAsync();

            return View(model);

        }
        public async Task<ActionResult> CargarExcel(int id = 0)
        {
            //var list = await db.ConceptoPpto.Select(x => x).ToListAsync();
            var model = new PryPacientesListViewModel();
            model.PresupuestoId = 0;
            model.PresupuestoId = 0;


            model.selectExcelTypes = await db.TipoCargaExcel.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.nombre
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
        public async Task<ActionResult> CargarExcel()
        {


            //IDataMaritimaDA oDataMaritimaDA = new DataMaritimaDA();
            var model = new CargaMasivaViewModel();
            String path = "";

            try
            {
  
                var datosSesion = Helper.SesionModel.DatosSesion;

                if (Request.Files.Count <= 0)
                {
                    model.Code = 14;
                    model.Message = "not file Selected";

                    return Json(new { model = model }, JsonRequestBehavior.AllowGet);
                }


                ////////////////////////////////////////////////////////////////////
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;

                HttpPostedFileBase file = files[0];

                path = getPath(file);


                 var listReader = ProcesarExcelPacientes(path);

                if (listReader.Count() == 0)
                {
                    model.Code = 14;
                    model.Message = "No se encontraron datos en la primera hoja.";
                    System.IO.File.Delete(path);
                    return Json(new { model = model }, JsonRequestBehavior.AllowGet);
                }
                else// vamos a agregar los 60 meses + los meses restantes del año base
                {
  
                    var mesBase = listReader.First().Mes;
                    var mesesFaltantesAnioBase = (12 - mesBase) + 1;
                    var mesesProyeccion = 60;
                    var totalMeses = mesesProyeccion + mesesFaltantesAnioBase;
                    var listProyecciones = new List<BulkPacientesViewModel>();



                    foreach (var item in listReader)
                    {
                       var list = GenerarPreyeccion(item, totalMeses);
                        listProyecciones.AddRange(list);

                    }

                    model.Data = listProyecciones;

                }
                var result = Request.Form["TipoID"];
                model.tipoInformacion =  int.Parse(Request.Form["TipoID"] ?? "0");
                model.id_escenario = long.Parse(Request.Form["EscenarioId"]);
                model.id_ppto = long.Parse(Request.Form["PresupuestoId"]);
                model.id_usuario = datosSesion.id_usuario;
                


                ICargaMasivaDA percistence = new CargaMasivaDA();
                var response = await percistence.CargaMasivaPacientes(model);


                System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                model.Code = 14;
                model.Message = "Exeption" + ex.Message;
                System.IO.File.Delete(path);
            }

            //model.Data = null;

            return Json(new { model = model }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public async Task<ActionResult> SaveBulkPppto(List<BulkPptoViewModel> bulkInsertList)
        {


            try
            {
                var datosSesion = Helper.SesionModel.DatosSesion;
                var listProyecciones = new List<BulkPptoViewModel>();
                var id = bulkInsertList[0].id_ppto;
                var ppto = await  db.Ppto.Where(x => x.id == id).FirstOrDefaultAsync();


                foreach (var item in bulkInsertList)
                {
                    
                    if(item.id_concepto == 1)
                    {
                        var mesesFaltantesAnioBase = (12 - item.Mes) + 1;
                        var mesesProyeccion = 60;
                        if (ppto.AInicial == item.Anio)
                        {
                            mesesFaltantesAnioBase = 0;
                        }
                            var totalMeses = mesesProyeccion + mesesFaltantesAnioBase;
                        if(ppto.AInicial == item.Anio && item.Mes > 1)
                        {
                            totalMeses = totalMeses - item.Mes;
                        }
                        var list = GenerarPreyeccionDesdeFormulario(item, totalMeses);
                        listProyecciones.AddRange(list);
                    }
                    else
                    {
                        listProyecciones.Add(item);
                    }
                    

                }
                long id_usuario = datosSesion.id_usuario;
                ICargaMasivaDA percistence = new CargaMasivaDA();
                var response = await percistence.CargaMasivaProyeccion(listProyecciones, id_usuario);
                return Json(new { errorCode = 0, merror = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { errorCode = 10, merror = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        private String getPath(HttpPostedFileBase file)
        {

            String pathTemp = ConfigurationManager.AppSettings["tempExcel"];


            if (!System.IO.Directory.Exists(pathTemp))
                System.IO.Directory.CreateDirectory(pathTemp);


            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            fileName = fileName.Replace(' ', '_');
            var extension = Path.GetExtension(file.FileName);

            string path = Path.Combine(pathTemp, String.Concat(fileName, String.Format("{0:_yyyyMMdd_HHmmss}", DateTime.Now), extension));

            file.SaveAs(path);

            //string path = Path.Combine(pathTemp, fileName);

            return path;
        }

        private IEnumerable<bulkData> ProcesarExcelPacientes(string path)
        {

            var listReader = new List<BulkPacientesViewModel>();
            using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    var ds = reader.AsDataSet();
                    var dt = ds.Tables[0];

                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty($"{dt.Rows[i]["Column0"]}") || string.IsNullOrEmpty($"{dt.Rows[i]["Column1"]}"))
                        {
                            continue;
                        }
                        else if (dt.Rows[i]["Column0"].ToString().Trim().Equals("TODOS"))
                        {
                            break;
                        }
                        var data = new BulkPacientesViewModel(dt, i);
                        listReader.Add(data);

                        //for (int j = 4; j < dt.Columns.Count; j++)
                        //{
                        //    if (dt.Rows[0][j].ToString().Trim().Length == 0) 
                        //    {
                        //        break;
                        //    }
                        //    var data = new BulkPacientesViewModel(dt, i);
                        //    data.Periodo = dt.Rows[0][j].ToString();

                        //    data.ValorPeriodo = data.GetDoubleValue(dt.Rows[i][j]);
                        //    listReader.Add(data);
                        //}



                    }
                }
            }

            return listReader;

        }

        private IEnumerable<BulkPptoViewModel> GenerarPreyeccionDesdeFormulario(BulkPptoViewModel item, int totalMeses)
        {

            var montoBase = item.Valor;

            var list = Enumerable.Range(0, totalMeses).Select(x => new BulkPptoViewModel
            {   // se crea desde cero para tomar el mes actual tambien
                id_cliente = item.id_cliente,
                id_concepto = item.id_concepto,
                id_escenario = item.id_escenario,
                id_localizacion = item.id_localizacion,
                id_ppto = item.id_ppto,
                Anio = new DateTime(item.Anio, item.Mes, 1).AddMonths(x).Year,
                Mes = new DateTime(item.Anio, item.Mes, 1).AddMonths(x).Month,
                Valor = Agregar5Porciento(ref montoBase, item.Anio, new DateTime(item.Anio, item.Mes, 1).AddMonths(x), totalMeses),
            }).ToList();


            return list;

        }
              private IEnumerable<BulkPacientesViewModel> GenerarPreyeccion(bulkData item, int totalMeses)
        {

            var montoBase = item.ValorPeriodo;

            var list = Enumerable.Range(0, totalMeses).Select(x => new BulkPacientesViewModel  // se crea desde cero para tomar el mes actual tambien
            {
                Cliente = item.Cliente,
                Ciudad = item.Ciudad,
                Contrato = item.Contrato,
                Periodo = item.Periodo,
                //tipoInformacion = item.tipoInformacion,
                Anio = new DateTime(item.Anio, item.Mes, 1).AddMonths(x).Year,
                Mes = new DateTime(item.Anio, item.Mes, 1).AddMonths(x).Month,
                ValorPeriodo = Agregar5Porciento(ref montoBase, item.Anio, new DateTime(item.Anio, item.Mes, 1).AddMonths(x), totalMeses)

            }).ToList();


            return list;

        }

        private double Agregar5Porciento(ref double montoBase,int anioBase,DateTime fechaLoop, int totalMeses)
        {
            var anioDif = fechaLoop.Year - anioBase;
            if (totalMeses > 60 && anioDif > 1)//mayor a dos porque recien al tercer año se añade el 5 %
            {
                if(fechaLoop.Month == 1)
                    montoBase = montoBase * 1.05;
            }else if(totalMeses <= 60 && anioDif > 0)
            {
                if (fechaLoop.Month == 1)
                    montoBase = montoBase * 1.05;
            }
            
            return montoBase;
        }

    }
}
