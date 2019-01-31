namespace NullHab.DAL.Models
{
    public class User
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public string PasswordHash { get; set; }
    }
}
