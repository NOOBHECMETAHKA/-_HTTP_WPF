using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class User
    {
        public int IdUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Email { get; set; } 
        public string Login { get; set; } 
        public string Password { get; set; } 
        public int PostId { get; set; }
        public bool? IsDeleted { get; set; }

        public User()
        {

        }

        public User(string firstName, string lastName, string email, string login, string password, int postId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
            PostId = postId;
        }

        public User(int idUser, string firstName, string lastName, string email, string login, string password, int postId, bool? isDeleted)
        {
            IdUser = idUser;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
            PostId = postId;
            IsDeleted = isDeleted;
        }
    }
}
