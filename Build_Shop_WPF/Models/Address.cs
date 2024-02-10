using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Address
    {
        public int? IdAddress { get; set; }
        public string City { get; set; } 
        public string Street { get; set; } 
        public string NumberBuild { get; set; } 
        public string Entrance { get; set; }
        public string NumberFlat { get; set; } 
        public string Сomment { get; set; } 
        public int СlientId { get; set; }
        public bool? IsDeleted { get; set; }

        public Address()
        {

        }

        public Address(string city, string street, string numberBuild, string entrance, string numberFlat, string сomment, int сlientId)
        {
            City = city;
            Street = street;
            NumberBuild = numberBuild;
            Entrance = entrance;
            NumberFlat = numberFlat;
            Сomment = сomment;
            СlientId = сlientId;
        }

        public Address(int? idAddress, string city, string street, string numberBuild, string entrance, string numberFlat, string сomment, int сlientId, bool? isDeleted)
        {
            IdAddress = idAddress;
            City = city;
            Street = street;
            NumberBuild = numberBuild;
            Entrance = entrance;
            NumberFlat = numberFlat;
            Сomment = сomment;
            СlientId = сlientId;
            IsDeleted = isDeleted;
        }
    }
}
