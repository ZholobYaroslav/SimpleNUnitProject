using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA_APP_Test_Project
{
    internal class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User(Guid id, string fullName, string password, string email)
        {
            Id = id;
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
        public User()
        {
            Id = Guid.Empty;
            FullName = "";
            Password = "";
            Email = "";
        }
        // better for User Account
        // public void RespondToPublication(Publication publication)
        // return PublicationService.Publications.Find(p => p == publication).UsersWhoResponded.Add(this);
    }
}
