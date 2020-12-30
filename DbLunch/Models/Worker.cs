using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLunch.Models
{
    public class Worker : BaseModel
    {
        public string name { get; set; }
        public Worker(int id, string name) : base(id)
        {
            this.name = name;
        }
    }
}
