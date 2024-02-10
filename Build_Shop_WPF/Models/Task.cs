using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Task
    {
        public int? IdTask { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public DateTime DateCreate { get; set; }
        public TimeSpan TimeCreate { get; set; }
        public DateTime? DateChange { get; set; }
        public TimeSpan? TimeChange { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public int ProjectId { get; set; }
        public int StatusId { get; set; }
        public bool? IsDeleted { get; set; }

        
    }
}
