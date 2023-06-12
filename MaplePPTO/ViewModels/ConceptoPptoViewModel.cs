using System;
using System.Collections.Generic;
using System.Globalization;

namespace MaplePPTO.ViewModels
{
    public class ConceptoPptoViewModel
    {
        public long ConceptoPptoId { get; set; }
        public string Presupuesto { get; set; }
        public string Escenario { get; set;}
        public string Fase { get; set; }
        public string Cliente { get; set; }
        public string Ciudad { get; set; }
        public string Concepto { get; set; }
        public string Concepto2 { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public double ValorPeriodo { get; set; }
        public string Contrato { get; set; }
        public IEnumerable<ConceptoPptoViewModel> listConcetoPptoVM { get; set; }

        public string Periodo
        {
            get 
            {
                var date = new DateTime(this.Anio, this.Mes, 1);
                var nombreMes = date.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));

                return $"{this.Anio} - {nombreMes}";
            }
            
        }
    }
}