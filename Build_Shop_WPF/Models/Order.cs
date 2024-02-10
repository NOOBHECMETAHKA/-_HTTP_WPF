using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Order
    {
        public int? IdOrder { get; set; }
        public DateTime DateCreate { get; set; }
        public TimeSpan TimeCreate { get; set; }
        public int ProjectId { get; set; }
        public int ContractorId { get; set; }
        public bool? IsDeleted { get; set; }

        public Order()
        {
        }

        public Order(DateTime dateCreate, TimeSpan timeCreate, int projectId, int contractorId)
        {
            DateCreate = dateCreate;
            TimeCreate = timeCreate;
            ProjectId = projectId;
            ContractorId = contractorId;
        }

        public Order(int? idOrder, DateTime dateCreate, TimeSpan timeCreate, int projectId, int contractorId, bool? isDeleted)
        {
            IdOrder = idOrder;
            DateCreate = dateCreate;
            TimeCreate = timeCreate;
            ProjectId = projectId;
            ContractorId = contractorId;
            IsDeleted = isDeleted;
        }
    }
}
