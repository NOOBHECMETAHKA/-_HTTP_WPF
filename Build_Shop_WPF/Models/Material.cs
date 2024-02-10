using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Material
    {
        public int? IdMaterial { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int WarehouseId { get; set; }
        public bool? IsDeleted { get; set; }

        public Material()
        {
        }

        public Material(string name, int amount, decimal price, int warehouseId)
        {
            Name = name;
            Amount = amount;
            Price = price;
            WarehouseId = warehouseId;
        }

        public Material(int? idMaterial, string name, int amount, decimal price, int warehouseId, bool? isDeleted)
        {
            IdMaterial = idMaterial;
            Name = name;
            Amount = amount;
            Price = price;
            WarehouseId = warehouseId;
            IsDeleted = isDeleted;
        }
    }
}
