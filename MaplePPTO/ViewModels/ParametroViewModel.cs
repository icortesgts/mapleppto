using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MaplePPTO.ViewModels
{

    public class ParametroListViewModel
    {
        public long parametroId { get; set; }
        public string parametroNombre { get; set; }
        public string valorstr { get; set; }
        public double valornum { get; set; }
        public DateTime? valordate { get; set; }
        public string Contrato { get; set; }
        public string Cliente { get; set; }
        public string Fase { get; set; }
        public string Escenario { get; set; }
    }
    public class ParametroViewModel
    {
        public long? PresupuestoId { get; set; }
        public int TipoReporteId { get; set; }
        public long? EscenarioId { get; set; }
        public int showMonths { get; set; }

        
        public List<ResultPpto> ListReports { get; set; }
      
        public List<ClienteParametroViewModel> listClientes {get;set;}
        public IEnumerable<SelectListItem> selectListBudgets { get; set; }
        public IEnumerable<SelectListItem> selectExcelTypes { get; set; }
        public IEnumerable<SelectListItem> selectListEscenaries { get; set; }
        public  IEnumerable<IGrouping<string,ParametroListViewModel>> ParametroList { get; set; }
        public List<SelectListItem> selectLisReportTypes { get; internal set; }
        public List<string> listHeader { get; internal set; }
        public List<string> listConceptParam { get; internal set; }
        public int mesesFaltantesAnioBase { get; set; }
        public long?[] selectedClients { get; set; }
        public int summaryReporType { get; set; }
        public List<ResultSummary> resultSummaries { get; set; }

    }
}