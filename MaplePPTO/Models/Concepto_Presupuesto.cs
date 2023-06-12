namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Concepto_Presupuesto
    {
        [Key]
        [Column(Order = 0)]
        public long id { get; set; }

        public long? orden { get; set; }

        public long? id_concepto { get; set; }

        public long? id_cliente { get; set; }

        public long? id_localizacion { get; set; }

        public long? id_escenario { get; set; }

        public long? id_ppto { get; set; }

        public short? tipo_reporte { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_base { get; set; }

        public double? mesant1 { get; set; }

        public double? mesant2 { get; set; }

        public double? mesant3 { get; set; }

        public double? mesant4 { get; set; }

        public double? mesant5 { get; set; }

        public double? mesant6 { get; set; }

        public double? mesant7 { get; set; }

        public double? mesant8 { get; set; }

        public double? mesant9 { get; set; }

        public double? mesant10 { get; set; }

        public double? mesant11 { get; set; }

        public double? mesant12 { get; set; }

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

        [Key]
        [Column(Order = 1)]
        public DateTime fecha_creacion { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long usu_crea { get; set; }

        public long? usu_modifica { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Concepto Concepto { get; set; }

        public virtual Escenario Escenario { get; set; }

        public virtual Localizacion Localizacion { get; set; }

        public virtual Ppto Ppto { get; set; }
    }
}
