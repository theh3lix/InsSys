namespace InsSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IC_idInInsuranceRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Insurance", "id_insuranceCompany", c => c.Int(nullable: false));
            CreateIndex("dbo.Insurance", "id_insuranceCompany");
            AddForeignKey("dbo.Insurance", "id_insuranceCompany", "dbo.InsuranceCompany", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Insurance", "id_insuranceCompany", "dbo.InsuranceCompany");
            DropIndex("dbo.Insurance", new[] { "id_insuranceCompany" });
            DropColumn("dbo.Insurance", "id_insuranceCompany");
        }
    }
}
