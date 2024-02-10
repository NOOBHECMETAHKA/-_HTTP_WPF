using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class CompositionEmployee
    {
        public int? IdCompositionEmployees { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public bool? IsDeleted { get; set; }

        public CompositionEmployee()
        {
            
        }

        public CompositionEmployee(int userId, int projectId)
        {
            UserId = userId;
            ProjectId = projectId;
        }

        public CompositionEmployee(int? idCompositionEmployees, int userId, int projectId, bool? isDeleted)
        {
            IdCompositionEmployees = idCompositionEmployees;
            UserId = userId;
            ProjectId = projectId;
            IsDeleted = isDeleted;
        }
    }
}
