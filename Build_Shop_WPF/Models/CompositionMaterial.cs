using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class CompositionMaterial
    {
        public int? IdCompositionMaterial { get; set; }
        public int MaterialId { get; set; }
        public int ProjectId { get; set; }
        public bool? IsDeleted { get; set; }

        public CompositionMaterial()
        {
        }

        public CompositionMaterial(int materialId, int projectId)
        {
            MaterialId = materialId;
            ProjectId = projectId;
        }

        public CompositionMaterial(int? idCompositionMaterial, int materialId, int projectId, bool? isDeleted)
        {
            IdCompositionMaterial = idCompositionMaterial;
            MaterialId = materialId;
            ProjectId = projectId;
            IsDeleted = isDeleted;
        }
    }
}
