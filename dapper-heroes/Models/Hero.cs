using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_heroes.Models
{
    public class Hero
    {
        public int id { get; set; }
        public string name { get; set; }
        public int power { get; set; }
        public int agility { get; set; }
    }
}
