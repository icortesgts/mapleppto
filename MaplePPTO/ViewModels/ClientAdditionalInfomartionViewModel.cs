using MaplePPTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaplePPTO.ViewModels
{
    public class ClientAdditionalInfomartionViewModel
    {
        public long clientId { get; set; }
        public long contratId { get; set; }

        public IEnumerable<Contrato> ContratosList { get; set; }
        public IEnumerable<ContratoLocalizacion> ContratoLocalizacionList { get; set; }
        public IEnumerable<ContratoFase> ContratoFaseList { get; set; }

    }
}