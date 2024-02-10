using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Contractor
    {

        public int? IdContractor { get; set; }
        public string Tin { get; set; }
        public string Ogrn { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string PhoneNumber { get; set; } 
        public bool? IsDeleted { get; set; }

        public Contractor()
        {
        }

        public Contractor(string tin, string ogrn, string firstName, string lastName, string phoneNumber)
        {
            Tin = tin;
            Ogrn = ogrn;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public Contractor(int idContractor, string tin, string ogrn, string firstName, string lastName, string phoneNumber, bool? isDeleted)
        {
            IdContractor = idContractor;
            Tin = tin;
            Ogrn = ogrn;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            IsDeleted = isDeleted;
        }
    }
}
