using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace MaplePPTO.ViewModels
{
    public class PryPacientesListViewModel
    {
        public long? PresupuestoId { get; set; }
        public long? EscenarioId { get; set; }
        public IEnumerable<SelectListItem> selectListBudgets { get; set; }
        public IEnumerable<SelectListItem> selectListEscenaries { get; set; }
        public List<SelectListItem> selectExcelTypes { get; internal set; }
    }
    
}