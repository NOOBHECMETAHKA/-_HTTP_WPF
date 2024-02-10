using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class CompositionEquipment
    {
        public int? IdCompositionEquipment { get; set; }
        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }
        public bool? IsDeleted { get; set; }

        public CompositionEquipment()
        {
        }

        public CompositionEquipment(int equipmentId, int projectId)
        {
            EquipmentId = equipmentId;
            ProjectId = projectId;
        }

        public CompositionEquipment(int? idCompositionEquipment, int equipmentId, int projectId, bool? isDeleted)
        {
            IdCompositionEquipment = idCompositionEquipment;
            EquipmentId = equipmentId;
            ProjectId = projectId;
            IsDeleted = isDeleted;
        }
    }
}
