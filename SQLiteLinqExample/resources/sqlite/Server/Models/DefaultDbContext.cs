using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.SQLite.EF6.Migrations;

namespace SQLiteLinqExample.resources.sqlite.Server.Models
{
    [DbConfigurationType(typeof(DatabaseConfiguration))]
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    public class ContextFactory : IDbContextFactory<DefaultDbContext>
    {
        public static string DatabaseFilePath;

        public static DefaultDbContext Instance
        {
            get
            {
                if (DatabaseFilePath == null) throw new InvalidOperationException("Tried to use database context before setting file path.\nMake sure you set the DB file path on the ContextFactory in the onResourceStart event, before using it!");

                return new DefaultDbContext("Data Source=" + DatabaseFilePath);
            }
        }

        public DefaultDbContext Create()
        {
            return Instance;
        }
    }

    internal sealed class MigrationConfiguration : DbMigrationsConfiguration<DefaultDbContext>
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }
    }

    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            return new SQLiteConnection(nameOrConnectionString);
        }
    }

    internal sealed class DatabaseConfiguration : DbConfiguration
    {
        public DatabaseConfiguration()
        {
            SetDefaultConnectionFactory(new SQLiteConnectionFactory());
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}
