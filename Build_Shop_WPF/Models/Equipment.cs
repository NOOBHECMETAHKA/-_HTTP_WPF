using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Equipment
    {
        public int? IdEquipment { get; set; }
        public string Name { get; set; } 
        public int Count { get; set; }
        public int WarehouseId { get; set; }
        public bool? IsDeleted { get; set; }

        public Equipment()
        {
        }

        public Equipment(string name, int count, int warehouseId)
        {
            Name = name;
            Count = count;
            WarehouseId = warehouseId;
        }

        public Equipment(int? idEquipment, string name, int count, int warehouseId, bool? isDeleted)
        {
            IdEquipment = idEquipment;
            Name = name;
            Count = count;
            WarehouseId = warehouseId;
            IsDeleted = isDeleted;
        }
    }
}
