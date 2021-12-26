namespace ShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhoneToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Mobile", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Mobile");
        }
    }
}
