namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InsumosDx")]
    public partial class InsumosDx
    {
        public int id { get; set; }

        [StringLength(200)]
        public string descripcion { get; set; }

        [StringLength(200)]
        public string presentacion { get; set; }

        public double? basl_leves { get; set; }

        public double? titulacion { get; set; }

        public double? nd { get; set; }

        public double? poligrafia { get; set; }

        public double? autoTitutacion { get; set; }

        public double? nivel2 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_crea { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_modifica { get; set; }

        public int? usuario_crea { get; set; }

        public int? usuario_modifica { get; set; }
    }
}
