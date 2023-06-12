namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContratoFase")]
    public partial class ContratoFase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContratoFase()
        {
            ConceptoPpto = new HashSet<ConceptoPpto>();
        }

        public long id { get; set; }

        public long id_contrato { get; set; }

        public long id_fase { get; set; }

        [StringLength(2000)]
        public string Condiciones { get; set; }

        [Column(TypeName = "money")]
        public decimal? TarifaPAciente { get; set; }

        public long? NroPAcientes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual Fase Fase { get; set; }
    }
}
