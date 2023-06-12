using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MaplePPTO.Models
{
    public partial class MAPLEEntities : DbContext
    {
        public MAPLEEntities()
            : base("name=MAPLEEntities")
        {
        }

        public virtual DbSet<Archivos> Archivos { get; set; }
        public virtual DbSet<Auditoria> Auditoria { get; set; }
        public virtual DbSet<Banco> Banco { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Concepto> Concepto { get; set; }
        public virtual DbSet<ConceptoPpto> ConceptoPpto { get; set; }
        public virtual DbSet<ConceptosPorTipoReporte> ConceptosPorTipoReporte { get; set; }
        public virtual DbSet<Contrato> Contrato { get; set; }
        public virtual DbSet<ContratoFase> ContratoFase { get; set; }
        public virtual DbSet<ContratoLocalizacion> ContratoLocalizacion { get; set; }
        public virtual DbSet<Escenario> Escenario { get; set; }
        public virtual DbSet<EscenarioPpto> EscenarioPpto { get; set; }
        public virtual DbSet<Fase> Fase { get; set; }
        public virtual DbSet<InsumosCext> InsumosCext { get; set; }
        public virtual DbSet<InsumosDx> InsumosDx { get; set; }
        public virtual DbSet<Localizacion> Localizacion { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Parametro> Parametro { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Ppto> Ppto { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TipoCargaExcel> TipoCargaExcel { get; set; }
        public virtual DbSet<TipoReporte> TipoReporte { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Concepto_Presupuesto> Concepto_Presupuesto { get; set; }
        public virtual DbSet<conceptopptodata> conceptopptodata { get; set; }
        public virtual DbSet<RESULPPTO> RESULPPTO { get; set; }
        public virtual DbSet<resulpptodata> resulpptodata { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Archivos>()
                .Property(e => e.tabla)
                .IsUnicode(false);

            modelBuilder.Entity<Archivos>()
                .Property(e => e.Archivo)
                .IsUnicode(false);

            modelBuilder.Entity<Archivos>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Auditoria>()
                .Property(e => e.Tabla)
                .IsUnicode(false);

            modelBuilder.Entity<Auditoria>()
                .Property(e => e.Original)
                .IsUnicode(false);

            modelBuilder.Entity<Auditoria>()
                .Property(e => e.Nuevo)
                .IsUnicode(false);

            modelBuilder.Entity<Auditoria>()
                .Property(e => e.Operacion)
                .IsUnicode(false);

            modelBuilder.Entity<Auditoria>()
                .Property(e => e.ipmac)
                .IsUnicode(false);

            modelBuilder.Entity<Banco>()
                .Property(e => e.Entidad)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.tipo_persona)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Tipo_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Telfono)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.emailR)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Concepto_Presupuesto)
                .WithOptional(e => e.Cliente)
                .HasForeignKey(e => e.id_cliente);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.Cliente)
                .HasForeignKey(e => e.id_cliente);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Contrato)
                .WithRequired(e => e.Cliente)
                .HasForeignKey(e => e.id_cliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Parametro)
                .WithOptional(e => e.Cliente)
                .HasForeignKey(e => e.id_cliente);

            modelBuilder.Entity<Concepto>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Concepto>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Concepto>()
                .Property(e => e.TipoPpto)
                .IsUnicode(false);

            modelBuilder.Entity<Concepto>()
                .HasMany(e => e.Concepto_Presupuesto)
                .WithOptional(e => e.Concepto)
                .HasForeignKey(e => e.id_concepto);

            modelBuilder.Entity<Concepto>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.Concepto)
                .HasForeignKey(e => e.id_concepto);

            modelBuilder.Entity<ConceptosPorTipoReporte>()
                .Property(e => e.Grupo)
                .IsUnicode(false);

            modelBuilder.Entity<Contrato>()
                .Property(e => e.Contrato1)
                .IsUnicode(false);

            modelBuilder.Entity<Contrato>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.Contrato)
                .HasForeignKey(e => e.id_contrato);

            modelBuilder.Entity<Contrato>()
                .HasMany(e => e.ContratoFase)
                .WithRequired(e => e.Contrato)
                .HasForeignKey(e => e.id_contrato)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contrato>()
                .HasMany(e => e.ContratoLocalizacion)
                .WithRequired(e => e.Contrato)
                .HasForeignKey(e => e.id_contrato)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContratoFase>()
                .Property(e => e.Condiciones)
                .IsUnicode(false);

            modelBuilder.Entity<ContratoFase>()
                .Property(e => e.TarifaPAciente)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ContratoFase>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.ContratoFase)
                .HasForeignKey(e => e.id_contratofase);

            modelBuilder.Entity<ContratoLocalizacion>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.ContratoLocalizacion)
                .HasForeignKey(e => e.id_contratolocalizacion);

            modelBuilder.Entity<Escenario>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Escenario>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Escenario>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.Escenario)
                .HasForeignKey(e => e.id_escenario);

            modelBuilder.Entity<Escenario>()
                .HasMany(e => e.Concepto_Presupuesto)
                .WithOptional(e => e.Escenario)
                .HasForeignKey(e => e.id_escenario);

            modelBuilder.Entity<Escenario>()
                .HasMany(e => e.EscenarioPpto)
                .WithRequired(e => e.Escenario)
                .HasForeignKey(e => e.id_escenario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Escenario>()
                .HasMany(e => e.Parametro)
                .WithOptional(e => e.Escenario)
                .HasForeignKey(e => e.id_escenario);

            modelBuilder.Entity<EscenarioPpto>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.EscenarioPpto)
                .HasForeignKey(e => e.id_Escenarioppto);

            modelBuilder.Entity<Fase>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Fase>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Fase>()
                .HasMany(e => e.Concepto)
                .WithOptional(e => e.Fase)
                .HasForeignKey(e => e.id_fase);

            modelBuilder.Entity<Fase>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.Fase)
                .HasForeignKey(e => e.id_fase);

            modelBuilder.Entity<Fase>()
                .HasMany(e => e.ContratoFase)
                .WithRequired(e => e.Fase)
                .HasForeignKey(e => e.id_fase)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Fase>()
                .HasMany(e => e.Parametro)
                .WithOptional(e => e.Fase)
                .HasForeignKey(e => e.id_fase);

            modelBuilder.Entity<InsumosCext>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<InsumosCext>()
                .Property(e => e.presentacion)
                .IsUnicode(false);

            modelBuilder.Entity<InsumosDx>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<InsumosDx>()
                .Property(e => e.presentacion)
                .IsUnicode(false);

            modelBuilder.Entity<Localizacion>()
                .Property(e => e.coddpto)
                .IsUnicode(false);

            modelBuilder.Entity<Localizacion>()
                .Property(e => e.NomDpto)
                .IsUnicode(false);

            modelBuilder.Entity<Localizacion>()
                .Property(e => e.ISODpto)
                .IsUnicode(false);

            modelBuilder.Entity<Localizacion>()
                .Property(e => e.CodMun)
                .IsUnicode(false);

            modelBuilder.Entity<Localizacion>()
                .Property(e => e.NomMun)
                .IsUnicode(false);

            modelBuilder.Entity<Localizacion>()
                .HasMany(e => e.Cliente)
                .WithRequired(e => e.Localizacion)
                .HasForeignKey(e => e.id_localizacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Localizacion>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.Localizacion)
                .HasForeignKey(e => e.id_localizacion);

            modelBuilder.Entity<Localizacion>()
                .HasMany(e => e.ContratoLocalizacion)
                .WithRequired(e => e.Localizacion)
                .HasForeignKey(e => e.id_localizacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Localizacion>()
                .HasMany(e => e.Concepto_Presupuesto)
                .WithOptional(e => e.Localizacion)
                .HasForeignKey(e => e.id_localizacion);

            modelBuilder.Entity<Pais>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Pais>()
                .Property(e => e.codISO)
                .IsUnicode(false);

            modelBuilder.Entity<Pais>()
                .HasMany(e => e.Localizacion)
                .WithRequired(e => e.Pais)
                .HasForeignKey(e => e.id_pais)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.ValorStr)
                .IsUnicode(false);

            modelBuilder.Entity<Perfil>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Perfil>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Perfil>()
                .HasMany(e => e.Usuarios)
                .WithRequired(e => e.Perfil)
                .HasForeignKey(e => e.id_perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ppto>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Ppto>()
                .HasMany(e => e.ConceptoPpto)
                .WithOptional(e => e.Ppto)
                .HasForeignKey(e => e.id_ppto);

            modelBuilder.Entity<Ppto>()
                .HasMany(e => e.EscenarioPpto)
                .WithRequired(e => e.Ppto)
                .HasForeignKey(e => e.id_presupuesto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ppto>()
                .HasMany(e => e.Parametro)
                .WithOptional(e => e.Ppto)
                .HasForeignKey(e => e.id_ppto);

            modelBuilder.Entity<Ppto>()
                .HasMany(e => e.Concepto_Presupuesto)
                .WithOptional(e => e.Ppto)
                .HasForeignKey(e => e.id_ppto);

            modelBuilder.Entity<TipoCargaExcel>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<TipoReporte>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Token)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.Ppto)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.Escenario)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.concepto)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.concept)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.ciudad)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.departamento)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.Contrato)
                .IsUnicode(false);

            modelBuilder.Entity<RESULPPTO>()
                .Property(e => e.Fase)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.Ppto)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.Escenario)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.concepto)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.concept)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.ciudad)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.departamento)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.Contrato)
                .IsUnicode(false);

            modelBuilder.Entity<resulpptodata>()
                .Property(e => e.Fase)
                .IsUnicode(false);
        }
    }
}
