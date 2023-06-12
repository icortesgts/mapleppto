namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConceptoPpto")]
    public partial class ConceptoPpto
    {
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

        public int Mes { get; set; }

        public int Vigencia { get; set; }

        public double valor { get; set; }

        public DateTime fecha_creacion { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        public long usu_crea { get; set; }

        public long? usu_modifica { get; set; }

        public short? tipoInformacion { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Concepto Concepto { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual ContratoFase ContratoFase { get; set; }

        public virtual ContratoLocalizacion ContratoLocalizacion { get; set; }

        public virtual Escenario Escenario { get; set; }

        public virtual EscenarioPpto EscenarioPpto { get; set; }

        public virtual Fase Fase { get; set; }

        public virtual Localizacion Localizacion { get; set; }

        public virtual Ppto Ppto { get; set; }
    }
}
