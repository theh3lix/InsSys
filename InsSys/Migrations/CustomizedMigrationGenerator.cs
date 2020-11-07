namespace InsSys.Migrations
{
    using MySql.Data.Entity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    public class CustomizedMigrationGenerator : MySqlMigrationSqlGenerator
    {

        protected override MigrationStatement Generate(CreateIndexOperation op)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                return base.Generate(op);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }
    }
}