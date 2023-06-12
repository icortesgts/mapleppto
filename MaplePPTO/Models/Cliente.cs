namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cliente")]
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            Concepto_Presupuesto = new HashSet<Concepto_Presupuesto>();
            ConceptoPpto = new HashSet<ConceptoPpto>();
            Contrato = new HashSet<Contrato>();
            Parametro = new HashSet<Parametro>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string tipo_persona { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Identificacion { get; set; }

        public int? DV { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(200)]
        public string Telfono { get; set; }

        [Required]
        [StringLength(200)]
        public string emailR { get; set; }

        public long id_localizacion { get; set; }

        [StringLength(200)]
        public string Tipo { get; set; }

        public bool? Prospecto { get; set; }

        public bool? Bloqueado { get; set; }

        public DateTime? fecha_crea { get; set; }

        public DateTime? fecha_modifica { get; set; }

        public long? usuario_crea { get; set; }

        public long? usuario_modifica { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Concepto_Presupuesto> Concepto_Presupuesto { get; set; }

        public virtual Localizacion Localizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contrato> Contrato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parametro> Parametro { get; set; }
    }
}
