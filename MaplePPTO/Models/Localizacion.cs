namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Localizacion")]
    public partial class Localizacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Localizacion()
        {
            Cliente = new HashSet<Cliente>();
            ConceptoPpto = new HashSet<ConceptoPpto>();
            ContratoLocalizacion = new HashSet<ContratoLocalizacion>();
            Concepto_Presupuesto = new HashSet<Concepto_Presupuesto>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(50)]
        public string coddpto { get; set; }

        [Required]
        [StringLength(200)]
        public string NomDpto { get; set; }

        [Required]
        [StringLength(50)]
        public string ISODpto { get; set; }

        [Required]
        [StringLength(50)]
        public string CodMun { get; set; }

        [Required]
        [StringLength(200)]
        public string NomMun { get; set; }

        public long id_pais { get; set; }

        public bool? Operacion { get; set; }

        public DateTime? fecha_crea { get; set; }

        public DateTime? fecha_modifica { get; set; }

        public long? usuario_crea { get; set; }

        public long? usuario_modifica { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cliente> Cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContratoLocalizacion> ContratoLocalizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Concepto_Presupuesto> Concepto_Presupuesto { get; set; }

        public virtual Pais Pais { get; set; }
    }
}
