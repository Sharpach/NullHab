using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NullHab.DAL.Providers.Identity
{
    public class UserClaimsTable : DapperTable
    {
        public UserClaimsTable(string connectionString) : base(connectionString)
        {
        }

        public async Task<ClaimsIdentity> FindByUserId(long userId)
        {
            var claims = new ClaimsIdentity();

            var sql = "SELECT * " +
                      "FROM dbo.UserClaim " +
                      "WHERE userid = @UserId;";

            using (var conn = OpenConnection(_connectionString))
            {
                foreach (var c in await conn.QueryAsync(sql, new { userId }))
                {
                    claims.AddClaim(new Claim(c.ClaimType, c.ClaimValue));
                }
            }

            return claims;
        }

        public async Task InsertAsync(Claim userClaim, long userId)
        {
            var sql = @"INSERT INTO dbo.UserClaim (ClaimValue, ClaimType, UserId) 
                values (@value, @type, @userId)";

            using (var conn = OpenConnection(_connectionString))
            {
                await conn.ExecuteAsync(sql, new
                {
                    value = userClaim.Value,
                    type = userClaim.Type,
                    userId
                });
            }
        }

        public async Task UpdateAsync(Claim oldClaim, Claim userClaim, long userId)
        {
            var sql = "UPDATE dbo.UserClaim " +
                      "SET claimtype=@NewClaimType, claimvalue=@NewClaimValue " +
                      "WHERE userid=@userId, claimtype=@OldClaimType, claimvalue=@OldClaimValue";

            using (var conn = OpenConnection(_connectionString))
            {
                await conn.ExecuteAsync(sql, new
                {
                    userId,
                    oldClaimType = oldClaim.Type,
                    OldClaimValue = oldClaim.Value,
                    NewClaimType = userClaim.Type,
                    NewClaimValue = userClaim.Value
                });
            }
        }

        public async Task DeleteAsync(long userId, Claim claim)
        {
            var sql = "DELETE FROM dbo.UserClaim " +
                      "WHERE UserId = @userId AND ClaimValue = @value AND ClaimType = @type";

            using (var conn = OpenConnection(_connectionString))
            {
                await conn.ExecuteAsync(sql, new { userId, value = claim.Value, type = claim.Type });
            }
        }

        public async Task<List<long>> GetUserIdsByClaim(Claim claim)
        {
            var sql = "SELECT userid " +
                      "FROM dbo.UserClaim " +
                      "WHERE ClaimValue = @value AND ClaimType = @type;";

            using (var conn = OpenConnection(_connectionString))
            {
                var userIds = await conn.QueryAsync<long>(sql, new { value = claim.Value, type = claim.Type });

                return userIds.ToList();
            }
        }
    }
}
