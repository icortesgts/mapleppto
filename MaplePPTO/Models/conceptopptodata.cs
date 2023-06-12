namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("conceptopptodata")]
    public partial class conceptopptodata
    {
        [Key]
        [Column(Order = 0)]
        public long id { get; set; }

        public long? orden { get; set; }

        public long? id_Escenarioppto { get; set; }

        public long? id_concepto { get; set; }

        public long? id_cliente { get; set; }

        public long? id_contrato { get; set; }

        public long? id_localizacion { get; set; }

        public long? id_contratofase { get; set; }

        public long? id_fase { get; set; }

        public long? id_escenario { get; set; }

        public long? id_ppto { get; set; }

        public long? id_contratolocalizacion { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Mes { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Vigencia { get; set; }

        [Key]
        [Column(Order = 3)]
        public double valor { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime fecha_creacion { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long usu_crea { get; set; }

        public long? usu_modifica { get; set; }

        public short? tipoInformacion { get; set; }
    }
}
