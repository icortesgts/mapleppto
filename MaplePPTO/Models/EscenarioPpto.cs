namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EscenarioPpto")]
    public partial class EscenarioPpto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EscenarioPpto()
        {
            ConceptoPpto = new HashSet<ConceptoPpto>();
        }

        public long id { get; set; }

        public long id_escenario { get; set; }

        public long id_presupuesto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConceptoPpto> ConceptoPpto { get; set; }

        public virtual Escenario Escenario { get; set; }

        public virtual Ppto Ppto { get; set; }
    }
}
