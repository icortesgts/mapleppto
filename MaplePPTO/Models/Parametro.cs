namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Parametro")]
    public partial class Parametro
    {
        public long id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string ValorStr { get; set; }

        public double? ValorNum { get; set; }

        public DateTime? ValorDate { get; set; }

        public DateTime? fecha_crea { get; set; }

        public DateTime? fecha_modifica { get; set; }

        public long? usuario_crea { get; set; }

        public long? usuario_modifica { get; set; }

        public long? id_fase { get; set; }

        public long? id_cliente { get; set; }

        public long? id_escenario { get; set; }

        public long? id_ppto { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Escenario Escenario { get; set; }

        public virtual Fase Fase { get; set; }

        public virtual Ppto Ppto { get; set; }
    }
}
