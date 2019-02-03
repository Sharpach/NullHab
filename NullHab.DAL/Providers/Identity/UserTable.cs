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

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            var sql = "DELETE FROM dbo.CustomUser WHERE Id = @Id";

            using (var conn = OpenConnection(_connectionString))
            {
                var rows = await conn.ExecuteAsync(sql, new { user.Id });

                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Email}." });
        }

        public async Task<User> FindByNameAsync(string normalizedUserName)
        {
            var sql = "SELECT * " +
                         "FROM dbo.CustomUser " +
                         "WHERE normalizedusername = @UserName;";

            using (var conn = OpenConnection(_connectionString))
            {
                return await conn.QuerySingleOrDefaultAsync<User>(sql, new
                {
                    UserName = normalizedUserName
                });
            }
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail)
        {
            var sql = "SELECT * " +
                      "FROM dbo.CustomUser " +
                      "WHERE normalizedemail = @Email;";

            using (var conn = OpenConnection(_connectionString))
            {
                return await conn.QuerySingleOrDefaultAsync<User>(sql, new
                {
                    Email = normalizedEmail
                });
            }
        }

        public async Task<User> FindByIdAsync(long id)
        {
            var sql = "SELECT * " +
                      "FROM dbo.CustomUser " +
                      "WHERE Id = @id;";

            using (var conn = OpenConnection(_connectionString))
            {
                return await conn.QuerySingleOrDefaultAsync<User>(sql, new
                {
                    id
                });
            }
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            var sql = "UPDATE dbo.customuser " +
                      "SET email = @Email, passwordhash = @PasswordHash, username = @UserName, normalizedusername = @NormalizedUserName, normalizedemail = @NormalizedEmail " +
                      "WHERE id = @Id;";

            using (var conn = OpenConnection(_connectionString))
            {
                var rows = await conn.ExecuteAsync(sql, new { user.Id, user.Email, user.NormalizedEmail, user.PasswordHash, user.UserName, user.NormalizedUserName });

                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not update user {user.Email}." });
        }
        
        private static IDbConnection OpenConnection(string connectionString)
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
