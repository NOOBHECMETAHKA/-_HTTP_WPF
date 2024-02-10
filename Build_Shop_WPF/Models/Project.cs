using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Project
    {

        public int? IdProject { get; set; }
        public string Name { get; set; } 
        public DateTime DateCreate { get; set; }
        public TimeSpan TimeCreate { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public int ClientId { get; set; }
        public bool? IsDeleted { get; set; }

        public Project()
        {

        }

        public Project(string name, DateTime dateCreate, TimeSpan timeCreate, DateTime dateEnd, TimeSpan timeEnd, int clientId)
        {
            Name = name;
            DateCreate = dateCreate;
            TimeCreate = timeCreate;
            DateEnd = dateEnd;
            TimeEnd = timeEnd;
            ClientId = clientId;
        }

        public Project(int idProject, string name, DateTime dateCreate, TimeSpan timeCreate, DateTime dateEnd, TimeSpan timeEnd, int clientId, bool? isDeleted)
        {
            IdProject = idProject;
            Name = name;
            DateCreate = dateCreate;
            TimeCreate = timeCreate;
            DateEnd = dateEnd;
            TimeEnd = timeEnd;
            ClientId = clientId;
            IsDeleted = isDeleted;
        }
    }
}
