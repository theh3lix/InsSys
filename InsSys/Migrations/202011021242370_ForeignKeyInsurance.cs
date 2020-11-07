namespace InsSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyInsurance : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Insurance", "Id_client");
            AddForeignKey("dbo.Insurance", "Id_client", "dbo.PersonalData", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Insurance", "Id_client", "dbo.PersonalData");
            DropIndex("dbo.Insurance", new[] { "Id_client" });
        }
    }
}
