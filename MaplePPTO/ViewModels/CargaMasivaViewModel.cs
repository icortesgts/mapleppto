using System.Collections.Generic;

namespace MaplePPTO.ViewModels
{
    public class CargaMasivaViewModel
    {
        public long id_usuario { get; set; }
        public long id_ppto { get; set; }
        public long id_escenario { get; set; }
        public int tipoInformacion { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<bulkData> Data { get; set; }
        
    }
}