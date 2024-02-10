using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Client
    {

        public int? IdClient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; } 
        public string Email { get; set; } 
        public bool? IsDeleted { get; set; }

        public Client()
        {

        }

        public Client(string firstName, string lastName, string phoneNumber, string email)
        {
            
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public Client(int? idClient, string firstName, string lastName, string phoneNumber, string email, bool? isDeleted)
        {
            IdClient = idClient;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            IsDeleted = isDeleted;
        }
    }
}
