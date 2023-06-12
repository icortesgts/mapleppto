namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConceptosPorTipoReporte")]
    public partial class ConceptosPorTipoReporte
    {
        public int id { get; set; }

        public int? tipoReporteId { get; set; }

        public long? conceptoId { get; set; }

        [StringLength(200)]
        public string Grupo { get; set; }

        public int? subGrupo { get; set; }

        public long? orden { get; set; }

        public virtual Concepto Concepto { get; set; }

        public virtual TipoReporte TipoReporte { get; set; }
    }
}
