namespace DatabaseFiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Multisigs", "BothUnsignedTx", c => c.String());
            AddColumn("dbo.Multisigs", "HubSignedTx", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Multisigs", "HubSignedTx");
            DropColumn("dbo.Multisigs", "BothUnsignedTx");
        }
    }
}
