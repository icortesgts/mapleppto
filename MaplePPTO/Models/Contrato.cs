namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contrato")]
    public partial class Contrato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contrato()
        {
            ConceptoPpto = new HashSet<ConceptoPpto>();
            ContratoFase = new HashSet<ContratoFase>();
            ContratoLocalizacion = new HashSet<ContratoLocalizacion>();
        }

        public long id { get; set; }

        [Column("Contrato")]
        [Required]
        [StringLength(200)]
        public string Contrato1 { get; set; }

        public DateTime FecIni { get; set; }

        public DateTime FecFin { get; set; }

        public long id_cliente { get; set; }

        public bool Prospecto { get; set; }

        public virtual Cliente Cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContratoFase> ContratoFase { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContratoLocalizacion> ContratoLocalizacion { get; set; }
    }
}
