namespace InsSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorizedUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(unicode: false),
                        Id_role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InsuranceCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ICName = c.String(unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InsurancePackage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageName = c.String(unicode: false),
                        PackageNo = c.Int(nullable: false),
                        Id_IC = c.Int(nullable: false),
                        ContributionSum = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Insurance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_client = c.Int(nullable: false),
                        InsurancePackageNo = c.Int(nullable: false),
                        InsuranceNr = c.String(unicode: false),
                        InsuranceStartDate = c.DateTime(nullable: false, precision: 0),
                        InsuranceEndDate = c.DateTime(nullable: false, precision: 0),
                        Status = c.String(unicode: false),
                        LastEditUser = c.String(unicode: false),
                        LastEditDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonalData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        DateOfBirth = c.DateTime(nullable: false, precision: 0),
                        PESEL = c.String(unicode: false),
                        City = c.String(unicode: false),
                        Street = c.String(unicode: false),
                        HouseNo = c.String(unicode: false),
                        LocalNo = c.String(unicode: false),
                        PostalCode = c.String(unicode: false),
                        PhoneNo = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role_name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRole");
            DropTable("dbo.PersonalData");
            DropTable("dbo.Insurance");
            DropTable("dbo.InsurancePackage");
            DropTable("dbo.InsuranceCompany");
            DropTable("dbo.AuthorizedUser");
        }
    }
}
