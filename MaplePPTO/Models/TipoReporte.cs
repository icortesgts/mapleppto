namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoReporte")]
    public partial class TipoReporte
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoReporte()
        {
            ConceptosPorTipoReporte = new HashSet<ConceptosPorTipoReporte>();
        }

        public int id { get; set; }

        [StringLength(400)]
        public string descripcion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_crea { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_modifica { get; set; }

        public int? usuario_crea { get; set; }

        public int? usuario_modifica { get; set; }

        public bool? esFinanciero { get; set; }

        public bool? deshabilitado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptosPorTipoReporte> ConceptosPorTipoReporte { get; set; }
    }
}
