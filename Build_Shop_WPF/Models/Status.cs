using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Status
    {

        public int? IdStatus { get; set; }
        public int LevelStatus { get; set; }
        public string NameStatus { get; set; } 
        public bool? IsDeleted { get; set; }

        public Status()
        {
        }

        public Status(int levelStatus, string nameStatus)
        {
            LevelStatus = levelStatus;
            NameStatus = nameStatus;
        }

        public Status(int idStatus, int levelStatus, string nameStatus, bool? isDeleted)
        {
            IdStatus = idStatus;
            LevelStatus = levelStatus;
            NameStatus = nameStatus;
            IsDeleted = isDeleted;
        }
    }
}
