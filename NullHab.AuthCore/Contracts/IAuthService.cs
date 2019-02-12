using NullHab.DAL.Models;
using System.Threading.Tasks;

namespace NullHab.AuthCore.Contracts
{
    public interface IAuthService
    {
        Task<User> Register(string email, string userName, string password);
        Task<string> Login(string email, string password);
    }
}