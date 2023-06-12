namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("resulpptodata")]
    public partial class resulpptodata
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(200)]
        public string Ppto { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string Escenario { get; set; }

        public long? id_concepto { get; set; }

        public long? id_localizacion { get; set; }

        public long? id_cliente { get; set; }

        public long? id_ppto { get; set; }

        public long? id_escenario { get; set; }

        public long? id_contrato { get; set; }

        public long? id_fase { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string concepto { get; set; }

        [StringLength(200)]
        public string concept { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(200)]
        public string ciudad { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(200)]
        public string departamento { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(200)]
        public string Cliente { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(200)]
        public string Contrato { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(200)]
        public string Fase { get; set; }

        public short? tipoinformacion { get; set; }

        public double? mesant6 { get; set; }

        public double? mesant5 { get; set; }

        public double? mesant4 { get; set; }

        public double? mesant3 { get; set; }

        public double? mesant2 { get; set; }

        public double? mesant1 { get; set; }

        public double? mes1 { get; set; }

        public double? mes2 { get; set; }

        public double? mes3 { get; set; }

        public double? mes4 { get; set; }

        public double? mes5 { get; set; }

        public double? mes6 { get; set; }

        public double? mes7 { get; set; }

        public double? mes8 { get; set; }

        public double? mes9 { get; set; }

        public double? mes10 { get; set; }

        public double? mes11 { get; set; }

        public double? mes12 { get; set; }

        public double? mes13 { get; set; }

        public double? mes14 { get; set; }

        public double? mes15 { get; set; }

        public double? mes16 { get; set; }

        public double? mes17 { get; set; }

        public double? mes18 { get; set; }

        public double? mes19 { get; set; }

        public double? mes20 { get; set; }

        public double? mes21 { get; set; }

        public double? mes22 { get; set; }

        public double? mes23 { get; set; }

        public double? mes24 { get; set; }

        public double? mes25 { get; set; }

        public double? mes26 { get; set; }

        public double? mes27 { get; set; }

        public double? mes28 { get; set; }

        public double? mes29 { get; set; }

        public double? mes30 { get; set; }

        public double? mes31 { get; set; }

        public double? mes32 { get; set; }

        public double? mes33 { get; set; }

        public double? mes34 { get; set; }

        public double? mes35 { get; set; }

        public double? mes36 { get; set; }

        public double? mes37 { get; set; }

        public double? mes38 { get; set; }

        public double? mes39 { get; set; }

        public double? mes40 { get; set; }

        public double? mes41 { get; set; }

        public double? mes42 { get; set; }

        public double? mes43 { get; set; }

        public double? mes44 { get; set; }

        public double? mes45 { get; set; }

        public double? mes46 { get; set; }

        public double? mes47 { get; set; }

        public double? mes48 { get; set; }

        public double? mes49 { get; set; }

        public double? mes50 { get; set; }

        public double? mes51 { get; set; }

        public double? mes52 { get; set; }

        public double? mes53 { get; set; }

        public double? mes54 { get; set; }

        public double? mes55 { get; set; }

        public double? mes56 { get; set; }

        public double? mes57 { get; set; }

        public double? mes58 { get; set; }

        public double? mes59 { get; set; }

        public double? mes60 { get; set; }

        public int? anio { get; set; }

        public int? mes { get; set; }

        public int? grupo { get; set; }

        public long? orden { get; set; }
    }
}
