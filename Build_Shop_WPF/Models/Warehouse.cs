using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Warehouse
    {

        public int IdWarehouse { get; set; }
        public string City { get; set; } 
        public string Street { get; set; } 
        public string NumberBuild { get; set; } 
        public bool? IsDeleted { get; set; }

        public Warehouse()
        {

        }

        public Warehouse(string city, string street, string numberBuild)
        {
            City = city;
            Street = street;
            NumberBuild = numberBuild;
        }

        public Warehouse(int idWarehouse, string city, string street, string numberBuild, bool? isDeleted)
        {
            IdWarehouse = idWarehouse;
            City = city;
            Street = street;
            NumberBuild = numberBuild;
            IsDeleted = isDeleted;
        }
    }
}
