namespace ShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromImageName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ImageName", c => c.String(nullable: false));
        }
    }
}
