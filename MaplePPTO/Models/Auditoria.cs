namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Auditoria")]
    public partial class Auditoria
    {
        public long id { get; set; }

        public long id_usuario { get; set; }

        [Required]
        [StringLength(200)]
        public string Tabla { get; set; }

        public long Llave { get; set; }

        public DateTime fecha { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Original { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Nuevo { get; set; }

        [Required]
        [StringLength(200)]
        public string Operacion { get; set; }

        [Required]
        [StringLength(50)]
        public string ipmac { get; set; }

        public DateTime? fecha_crea { get; set; }

        public DateTime? fecha_modifica { get; set; }

        public long? usuario_crea { get; set; }

        public long? usuario_modifica { get; set; }

        public long? id_cliente { get; set; }
    }
}
