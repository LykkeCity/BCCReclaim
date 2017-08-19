namespace DatabaseFiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmountsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Multisigs", "clientAmount", c => c.String());
            AddColumn("dbo.Multisigs", "hubAmount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Multisigs", "hubAmount");
            DropColumn("dbo.Multisigs", "clientAmount");
        }
    }
}
