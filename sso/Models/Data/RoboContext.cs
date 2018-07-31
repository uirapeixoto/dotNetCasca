namespace sso.Models.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RoboContext : DbContext
    {
        public RoboContext()
            : base("name=RoboContext")
        {
        }

        public RoboContext(AmbienteModel ambiente)
            : base(string.Format("name={0}", ambiente.Context))
        {

        }

        public virtual DbSet<TB_LOGIN_ROBO> TB_LOGIN_ROBO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TB_LOGIN_ROBO>()
                .Property(e => e.DS_NOME)
                .IsUnicode(false);

            modelBuilder.Entity<TB_LOGIN_ROBO>()
                .Property(e => e.DS_SENHA)
                .IsUnicode(false);

            modelBuilder.Entity<TB_LOGIN_ROBO>()
                .Property(e => e.DS_CONVENIO)
                .IsUnicode(false);

            modelBuilder.Entity<TB_LOGIN_ROBO>()
                .Property(e => e.DS_PROPOSTA_UF)
                .IsUnicode(false);

            modelBuilder.Entity<TB_LOGIN_ROBO>()
                .Property(e => e.DS_SISTEMA)
                .IsUnicode(false);

            modelBuilder.Entity<TB_LOGIN_ROBO>()
                .Property(e => e.DS_RESPONSAVEL)
                .IsUnicode(false);
        }
    }
}
