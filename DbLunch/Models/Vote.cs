using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLunch.Models
{
    public class Vote :BaseModel
    {
        public DateTime date { get; set; }
        public int voter_id { get; set; }
        public int restaurant_id { get; set; }

        public Vote(DateTime date, int voter_id, int restaurant_id, int id) : base(id)
        {
            this.date = date;
            this.voter_id = voter_id;
            this.restaurant_id = restaurant_id;
        }
    }
}
