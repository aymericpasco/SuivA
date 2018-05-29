using System.Configuration;
using System.Data.Entity;
using MySql.Data.Entity;

namespace SuivA.Data.Entity
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataContext : DbContext
    {
        private static string _dbHost;

        // DEBUG
        /*public DataContext() : base(GetConnectionString())
        {
        }*/

        // PROD
        public DataContext() : base("name=ApplicationFrais")
        {
            
        }


        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Office> Offices { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Visit> Visits { get; set; }

        public static void SetDatabaseHost(string dbHost)
        {
            _dbHost = dbHost;
        }

        public static string GetConnectionString()
        {
            var connString = ConfigurationManager.ConnectionStrings["ApplicationFraisDebug"].ConnectionString;


            return string.Format(connString, _dbHost);
        }
    }
}