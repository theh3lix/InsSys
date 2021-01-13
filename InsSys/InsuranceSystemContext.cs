using InsSys.Models;
using MySql.Data.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace InsSys
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class InsuranceSystemContext : DbContext
    {
        public InsuranceSystemContext() : base("DB")
        {

        }
        public DbSet<AuthorizedUser> AuthorizedUsers { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public DbSet<InsurancePackage> InsurancePackages { get; set; }
        public DbSet<PersonalData> PersonalData { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}