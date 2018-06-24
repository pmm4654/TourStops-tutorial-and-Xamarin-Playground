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

        public override bool Equals(object obj)
        {
            var other = obj as Thing;
            if (other == null)
                return false;

            return this.id == other.id;
        }
    }
}
