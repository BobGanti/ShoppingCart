namespace ShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnnotationsToPages : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pages", "Title", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Pages", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pages", "Body", c => c.String());
            AlterColumn("dbo.Pages", "Title", c => c.String(maxLength: 50));
        }
    }
}
