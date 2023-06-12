namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Archivos
    {
        public long id { get; set; }

        [Required]
        [StringLength(200)]
        public string tabla { get; set; }

        public long llave { get; set; }

        [Required]
        [StringLength(200)]
        public string Archivo { get; set; }

        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime? fecha_crea { get; set; }

        public DateTime? fecha_modifica { get; set; }

        public long? usuario_crea { get; set; }

        public long? usuario_modifica { get; set; }
    }
}
