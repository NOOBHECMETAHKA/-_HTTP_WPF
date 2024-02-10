using System;
using System.Collections.Generic;

namespace Build_Shop_WPF.Models
{
    public partial class Post
    {

        public int? IdPost { get; set; }
        public string Name { get; set; } 
        public decimal Salary { get; set; }
        public bool? IsDeleted { get; set; }

        public Post()
        {
        }

        public Post(string name, decimal salary)
        {
            Name = name;
            Salary = salary;
        }

        public Post(int? idPost, string name, decimal salary, bool? isDeleted)
        {
            IdPost = idPost;
            Name = name;
            Salary = salary;
            IsDeleted = isDeleted;
        }
    }
}
