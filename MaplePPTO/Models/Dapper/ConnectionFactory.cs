using System.Configuration;
using System.Data;
using System.Data.Common;

namespace MaplePPTO.Models.Dapper
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MAPLEDapper"].ConnectionString;
        public IDbConnection GetConnection
        {
            get
            {
                var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                var conn = factory.CreateConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
                return conn;
            }
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}