using Npgsql;
using System.Data;

namespace NullHab.DAL.Providers
{
    public class DapperTable
    {
        protected readonly string _connectionString;

        public DapperTable(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected static IDbConnection OpenConnection(string connectionString)
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
