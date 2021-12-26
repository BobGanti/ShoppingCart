namespace ShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeddAdmin : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Users (Firstname, Lastname, Email, Username, Password, Mobile) VALUES ('Super', 'Administrator', 'admin@ntsc.com', 'admin', 'password', '123456789')");
        }
        
        public override void Down()
        {
        }
    }
}
