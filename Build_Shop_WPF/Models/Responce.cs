using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build_Shop_WPF.Models
{
    public class Responce
    {
        public string access_token { get; set; }
        public string username { get; set; }

        public Responce()
        {

        }

        public Responce(string access_token, string username)
        {
            this.access_token = access_token;
            this.username = username;
        }
    }
}
