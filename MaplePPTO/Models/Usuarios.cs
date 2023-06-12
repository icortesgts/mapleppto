namespace MaplePPTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Usuarios
    {
        [Required]
        [StringLength(50)]
        public string login { get; set; }

        [Required]
        [MaxLength(50)]
        public byte[] Password { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        public bool Bloqueado { get; set; }

        public DateTime Fecha_Password { get; set; }

        public DateTime Fecha_Creacion { get; set; }

        public DateTime Fecha_Modificacion { get; set; }

        public long id_perfil { get; set; }

        public long Id { get; set; }

        [StringLength(255)]
        public string Token { get; set; }

        public DateTime? fecha_crea { get; set; }

        public DateTime? fecha_modifica { get; set; }

        public long? usuario_crea { get; set; }

        public long? usuario_modifica { get; set; }

        public virtual Perfil Perfil { get; set; }
    }
}
