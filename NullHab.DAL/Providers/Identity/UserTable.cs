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
            var sql = "INSERT INTO dbo.CustomUser (Email, NormalizedEmail, PasswordHash, UserName, NormalizedUserName)" +
                         "VALUES (@Email, @NormalizedEmail, @PasswordHash, @UserName, @NormalizedUserName)";

            using (var conn = OpenConnection(_connectionString))
            {
                var rows = await conn.ExecuteAsync(sql, new { user.Email, user.NormalizedEmail, user.PasswordHash, user.UserName, user.NormalizedUserName });

                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
        }

        public async Task<User> FindByNameAsync(string normalizedUserName)
        {
            var sql = "SELECT * " +
                         "FROM dbo.CustomUser " +
                         "WHERE UserName = @UserName;";

            using (var conn = OpenConnection(_connectionString))
            {
                return await conn.QuerySingleOrDefaultAsync<User>(sql, new
                {
                    UserName = normalizedUserName
                });
            }
        }

        private static IDbConnection OpenConnection(string connStr)
        {
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            return conn;
        }
    }
}
