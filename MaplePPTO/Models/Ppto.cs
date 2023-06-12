namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ppto")]
    public partial class Ppto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ppto()
        {
            ConceptoPpto = new HashSet<ConceptoPpto>();
            EscenarioPpto = new HashSet<EscenarioPpto>();
            Parametro = new HashSet<Parametro>();
            Concepto_Presupuesto = new HashSet<Concepto_Presupuesto>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        public int AInicial { get; set; }

        public int AFinal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EscenarioPpto> EscenarioPpto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parametro> Parametro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Concepto_Presupuesto> Concepto_Presupuesto { get; set; }
    }
}
