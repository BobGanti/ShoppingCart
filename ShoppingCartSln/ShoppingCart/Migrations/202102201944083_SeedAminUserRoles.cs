namespace ShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedAminUserRoles : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 1)");
        }
        
        public override void Down()
        {
        }
    }
}
