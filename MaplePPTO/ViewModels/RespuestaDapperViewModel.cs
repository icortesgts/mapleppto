﻿using System;
using System.Collections.Generic;

namespace MaplePPTO.ViewModels
{
    public class RespuestaDapperViewModel
    {
        public int errorCode { get; set; }

        public string merror { get; set; }

        public List<ResultPpto> reports { get; set; }

        public List<ResultSummary> resultSummaries { get; set; }

        public List<ResultPptoFinancieroVm> resultFinanciero { get; set; }
    }

    public class ResultPpto
    {
        public int grupo { get; set; }

        public int subgrupo { get; set; }

        public string Ppto { get; set; }

        public string Escenario { get; set; }

        public string concepto { get; set; }

        public string ciudad { get; set; }

        public string Cliente { get; set; }

        public string Contrato { get; set; }

        public string Fase { get; set; }

        public string mesant6 { get; set; } = "0";

        public string mesant5 { get; set; } = "0";

        public string mesant4 { get; set; } = "0";

        public string mesant3 { get; set; } = "0";

        public string mesant2 { get; set; } = "0";

        public string mesant1 { get; set; } = "0";

        public string mes1 { get; set; } = "0";

        public string mes2 { get; set; } = "0";

        public string mes3 { get; set; } = "0";

        public string mes4 { get; set; } = "0";

        public string mes5 { get; set; } = "0";

        public string mes6 { get; set; } = "0";

        public string mes7 { get; set; } = "0";

        public string mes8 { get; set; } = "0";

        public string mes9 { get; set; } = "0";

        public string mes10 { get; set; } = "0";

        public string mes11 { get; set; } = "0";

        public string mes12 { get; set; } = "0";

        public string mes13 { get; set; } = "0";

        public string mes14 { get; set; } = "0";

        public string mes15 { get; set; } = "0";

        public string mes16 { get; set; } = "0";

        public string mes17 { get; set; } = "0";

        public string mes18 { get; set; } = "0";

        public string mes19 { get; set; } = "0";

        public string mes20 { get; set; } = "0";

        public string mes21 { get; set; } = "0";

        public string mes22 { get; set; } = "0";

        public string mes23 { get; set; } = "0";

        public string mes24 { get; set; } = "0";

        public string mes25 { get; set; } = "0";

        public string mes26 { get; set; } = "0";

        public string mes27 { get; set; } = "0";

        public string mes28 { get; set; } = "0";

        public string mes29 { get; set; } = "0";

        public string mes30 { get; set; } = "0";

        public string mes31 { get; set; } = "0";

        public string mes32 { get; set; } = "0";

        public string mes33 { get; set; } = "0";

        public string mes34 { get; set; } = "0";

        public string mes35 { get; set; } = "0";

        public string mes36 { get; set; } = "0";

        public string mes37 { get; set; } = "0";

        public string mes38 { get; set; } = "0";

        public string mes39 { get; set; } = "0";

        public string mes40 { get; set; } = "0";

        public string mes41 { get; set; } = "0";

        public string mes42 { get; set; } = "0";

        public string mes43 { get; set; } = "0";

        public string mes44 { get; set; } = "0";

        public string mes45 { get; set; } = "0";

        public string mes46 { get; set; } = "0";

        public string mes47 { get; set; } = "0";

        public string mes48 { get; set; } = "0";

        public string mes49 { get; set; } = "0";

        public string mes50 { get; set; } = "0";

        public string mes51 { get; set; } = "0";

        public string mes52 { get; set; } = "0";

        public string mes53 { get; set; } = "0";

        public string mes54 { get; set; } = "0";

        public string mes55 { get; set; } = "0";

        public string mes56 { get; set; } = "0";

        public string mes57 { get; set; } = "0";

        public string mes58 { get; set; } = "0";

        public string mes59 { get; set; } = "0";

        public string mes60 { get; set; } = "0";

        public int anio { get; set; }

        public int mes { get; set; }
    }

    public class ResultSummary
    {
        public string Nombre { get; set; }

        public DateTime fecha { get; set; }

        public Decimal cop { get; set; }

        public Decimal usd { get; set; }

        public Decimal porcentaje { get; set; }

        public int orden { get; set; }

    }
}