namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Concepto")]
    public partial class Concepto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Concepto()
        {
            Concepto_Presupuesto = new HashSet<Concepto_Presupuesto>();
            ConceptosPorTipoReporte = new HashSet<ConceptosPorTipoReporte>();
            ConceptoPpto = new HashSet<ConceptoPpto>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public bool Entrada { get; set; }

        [Required]
        [StringLength(200)]
        public string TipoPpto { get; set; }

        public long? id_fase { get; set; }

        public long? orden { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Concepto_Presupuesto> Concepto_Presupuesto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptosPorTipoReporte> ConceptosPorTipoReporte { get; set; }

        public virtual Fase Fase { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }
    }
}
