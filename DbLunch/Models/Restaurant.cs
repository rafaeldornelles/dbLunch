using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLunch.Models
{
    public class Restaurant: BaseModel
    {
        public string Name { get; set; }

        public Restaurant(int id, string name) : base(id)
        {
            Name = name;
        }
    }
}
