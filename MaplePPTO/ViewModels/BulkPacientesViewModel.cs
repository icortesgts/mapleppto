using System;
using System.Data;

namespace MaplePPTO.ViewModels
{
    public interface bulkData
    {
         string Cliente { get; set; }
         string Ciudad { get; set; }
         string Contrato { get; set; }
         string Concepto { get; set; }
         string Concepto2 { get; set; }
         string Periodo { get; set; }
         double ValorPeriodo { get; set; }
        int Anio { get; set; }
        int Mes { get; set; }
        int tipoInformacion { get; set; }

    }
    public class BulkPacientesViewModel: bulkData
    {
        public string Cliente { get; set; }
        public string Ciudad { get; set; }
        public string Concepto { get; set; }
        public string Concepto2 { get; set; }
        public string Periodo { get; set; }//base 
        public double ValorPeriodo { get; set;}
        public string Contrato { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int tipoInformacion { get; set; }

        public BulkPacientesViewModel()
        {

        }
        public BulkPacientesViewModel(DataTable dt, int index)
        {
            this.Cliente = dt.Rows[index]["Column0"].ToString();
            this.Ciudad = dt.Rows[index]["Column1"].ToString();
            this.Contrato = dt.Rows[index]["Column2"].ToString();
            this.Periodo = dt.Rows[0]["Column3"].ToString();
            this.ValorPeriodo = this.GetDoubleValue(dt.Rows[index]["Column3"]);
            this.Anio = this.GetAnioPeriodo();
            this.Mes = this.GetMesPeriodo();
        }

        private double GetDoubleValue(object obj)
        {

            return (obj != null &&
                    obj.ToString().Trim().Length > 0 ?
                    double.Parse(obj.ToString().Trim()) : 0);

        }
        private int GetAnioPeriodo()
        {
            var fecha = DateTime.Parse(this.Periodo);

            return fecha.Year;
        }
        private int GetMesPeriodo()
        {
            var fecha = DateTime.Parse(this.Periodo);

            return fecha.Month;
        }

    }

    public class BulkPptoViewModel
    {
        public long id_concepto { get; set; }
        public long usu_crea { get; set; }
        public long id_cliente { get; set; }
        public long id_localizacion { get; set; }
        public long id_escenario { get; set; }
        public long id_ppto { get; set; }
        public double Valor { get; set; }
        public DateTime? fecha { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
    }

}