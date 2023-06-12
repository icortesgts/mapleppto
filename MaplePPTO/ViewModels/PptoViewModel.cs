using MaplePPTO.Models;
using System.Collections.Generic;

namespace MaplePPTO.ViewModels
{
    public class PptoViewModel
    {

        public long id { get; set; }
        public string Nombre { get; set; }
        public int AInicial { get; set; }
        public int AFinal { get; set; }
        public long EscenarioId { get; set; }

        public  List<Escenario> Escenario { get; set; }
        public List<Ppto> Ppto { get; set; }
    }

    public class PptoListViewModel
    {

        public long id { get; set; }
        public string Nombre { get; set; }
        public int AInicial { get; set; }
        public int AFinal { get; set; }
        public long EscenarioPptoId { get; set; }
        public string EscenarioNombre { get; set; } 

    }
}