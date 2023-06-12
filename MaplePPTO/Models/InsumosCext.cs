namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InsumosCext")]
    public partial class InsumosCext
    {
        public int id { get; set; }

        [StringLength(200)]
        public string descripcion { get; set; }

        [StringLength(200)]
        public string presentacion { get; set; }

        public double? mExperto { get; set; }

        public double? somnlogo { get; set; }

        public double? tRespiratorio { get; set; }

        public double? nutricion { get; set; }

        public double? psicologia { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_crea { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_modifica { get; set; }

        public int? usuario_crea { get; set; }

        public int? usuario_modifica { get; set; }
    }
}
