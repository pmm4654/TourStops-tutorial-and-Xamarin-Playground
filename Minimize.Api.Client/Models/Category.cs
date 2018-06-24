using System;
using System.Collections.Generic;
using System.Text;

namespace Minimize.Api.Client.Models
{
    public class Category
    {
        public string name { get; set; }
        public int user_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }    
        public int threshold { get; set; }
    }
}
