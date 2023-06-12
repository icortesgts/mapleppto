namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Escenario")]
    public partial class Escenario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Escenario()
        {
            ConceptoPpto = new HashSet<ConceptoPpto>();
            Concepto_Presupuesto = new HashSet<Concepto_Presupuesto>();
            EscenarioPpto = new HashSet<EscenarioPpto>();
            Parametro = new HashSet<Parametro>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(2000)]
        public string Descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Concepto_Presupuesto> Concepto_Presupuesto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EscenarioPpto> EscenarioPpto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parametro> Parametro { get; set; }
    }
}
