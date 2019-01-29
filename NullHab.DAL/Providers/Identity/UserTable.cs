using Dapper;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using NullHab.DAL.Models;
using System.Data;
using System.Threading.Tasks;

namespace NullHab.DAL.Providers.Identity
{
    public class UserTable
    {
        private readonly string _connectionString;

        public UserTable(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IdentityResult> CreateAsync(User user)
        {
            var sql = "INSERT INTO dbo.CustomUser " +
                         "VALUES (@id, @Email, @EmailConfirmed, @PasswordHash, @UserName)";

            using (var conn = OpenConnection(_connectionString))
            {
                conn.Open();

                var rows = await conn.ExecuteAsync(sql, new { user.Id, user.Email, user.PasswordHash, user.UserName });

                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
        }

        private static IDbConnection OpenConnection(string connStr)
        {
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            return conn;
        }
    }
}
