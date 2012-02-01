using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace Sura.Models
{
    public class User
    {
        public string Id { get; protected internal set; }

        private string Password { get; set; }
        private Guid Salt { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }

        public DateTimeOffset CreatedAt { get; protected internal set; }
        public DateTimeOffset? LastLoginAt { get; set; }
        public int LoginFailureCount { get; set; }
        public DateTimeOffset? LastLoginFailureAt { get; set; }
        public DateTimeOffset? LastLockoutAt { get; set; }

        public User() { }

        public User(string username, string password)
        {
            Id = username;

            CreatedAt = DateTimeOffset.UtcNow;
            Salt = Guid.NewGuid();
            Password = HashPassword(password, CreatedAt, Salt);
        }

        public void ChangePassword(string password)
        {
            Salt = Guid.NewGuid();
            Password = HashPassword(password, CreatedAt, Salt);
        }

        public bool Authenticate(string password)
        {
            return HashPassword(password, CreatedAt, Salt) == Password;
        }

        private static string HashPassword(string password, DateTimeOffset creationDate, Guid passwordSalt)
        {
            string hashedPassword;
            
            using (var sha = SHA512.Create())
            {
                var computedHash = sha.ComputeHash(
                    passwordSalt.ToByteArray().Concat(
                        Encoding.Unicode.GetBytes(creationDate + password + WebConfigurationManager.AppSettings["Sura-Users-Hash"])
                        ).ToArray()
                    );

                hashedPassword = Convert.ToBase64String(computedHash);
            }

            return hashedPassword;
        }
    }
}