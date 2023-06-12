namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContratoLocalizacion")]
    public partial class ContratoLocalizacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContratoLocalizacion()
        {
            ConceptoPpto = new HashSet<ConceptoPpto>();
        }

        public long id { get; set; }

        public long id_contrato { get; set; }

        public long id_localizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual Localizacion Localizacion { get; set; }
    }
}
