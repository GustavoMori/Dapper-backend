using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_heroes.Models
{
    public class TeamHero
    {
        public int hero_idfk { get; set; }
        public int team_idfk { get; set; }
    }

    public class TeamHeroResponse
    {
        public Hero hero { get; set; }
        public Team team { get; set; }
    }

}
