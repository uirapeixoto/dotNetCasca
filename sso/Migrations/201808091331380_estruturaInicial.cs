namespace sso.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estruturaInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_LOGIN_ROBO",
                c => new
                    {
                        CO_SEQ_USUARIO = c.Int(nullable: false, identity: true),
                        DS_NOME = c.String(nullable: false, maxLength: 60, unicode: false),
                        DS_SENHA = c.String(nullable: false, maxLength: 50, unicode: false),
                        DS_CONVENIO = c.String(maxLength: 50, unicode: false),
                        DT_EXECUCAO = c.DateTime(),
                        DT_DESATIVACAO = c.DateTime(),
                        DS_PROPOSTA_UF = c.String(nullable: false, maxLength: 2, unicode: false),
                        DS_SISTEMA = c.String(nullable: false, maxLength: 50, unicode: false),
                        ST_EMMANUTENCAO = c.Boolean(nullable: false),
                        ST_DESBLOQUEAR = c.Boolean(nullable: false),
                        DS_RESPONSAVEL = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.CO_SEQ_USUARIO);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TB_LOGIN_ROBO");
        }
    }
}
