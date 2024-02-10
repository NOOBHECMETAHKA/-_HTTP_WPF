using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Consumption
    {
        public int? IdConsumption { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePayment { get; set; }
        public DateTime TimePayment { get; set; }
        public int ProjectId { get; set; }
        public bool? IsDeleted { get; set; }

        public Consumption()
        {
        }

        public Consumption(string name, string description, decimal amount, DateTime datePayment, DateTime timePayment, int projectId)
        {
            Name = name;
            Description = description;
            Amount = amount;
            DatePayment = datePayment;
            TimePayment = timePayment;
            ProjectId = projectId;
        }

        public Consumption(int idConsumption, string name, string description, decimal amount, DateTime datePayment, DateTime timePayment, int projectId, bool? isDeleted)
        {
            IdConsumption = idConsumption;
            Name = name;
            Description = description;
            Amount = amount;
            DatePayment = datePayment;
            TimePayment = timePayment;
            ProjectId = projectId;
            IsDeleted = isDeleted;
        }
    }
}
