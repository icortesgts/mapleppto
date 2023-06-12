namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoCargaExcel")]
    public partial class TipoCargaExcel
    {
        public int id { get; set; }

        [StringLength(100)]
        public string nombre { get; set; }

        public bool? esOperativo { get; set; }
    }
}
