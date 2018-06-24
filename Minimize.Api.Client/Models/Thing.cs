using System;
using System.Collections.Generic;
using System.Text;

namespace Minimize.Api.Client.Models
{
    public class Thing
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime? date_last_used { get; set; }
        public int user_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int category_id { get; set; }
        public byte[] image { get; set; }
    }
}
