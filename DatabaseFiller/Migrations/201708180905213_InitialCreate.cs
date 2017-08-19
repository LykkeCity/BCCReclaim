namespace DatabaseFiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Multisigs",
                c => new
                    {
                        MultsigAddress = c.String(nullable: false, maxLength: 128),
                        Pubkey01 = c.String(),
                        Pubkey02 = c.String(),
                    })
                .PrimaryKey(t => t.MultsigAddress);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Multisigs");
        }
    }
}
