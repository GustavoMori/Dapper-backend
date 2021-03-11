using dapper_heroes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_heroes.Controllers
{
    [Route("api/teamhero")]
    [ApiController]
    public class TeamHeroController : ControllerBase
    {
        private readonly TeamHeroRepository teamHeroRepository;

        public TeamHeroController()
        {
            teamHeroRepository = new TeamHeroRepository();
        }

        [HttpGet]
        public IEnumerable<TeamHero> GetAllRelationship()
        {
            return teamHeroRepository.GetAllRelationship();
        }

        [HttpGet("{team_idfk}")]
        public TeamHero GetByTeamIdFk(int team_idfk)
        {
            return teamHeroRepository.GetByTeamIdFk(team_idfk);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TeamHero teamHero)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTeamHeroRelationship = teamHeroRepository.Add(teamHero);
                    return Ok(newTeamHeroRelationship);
                }

                return BadRequest("falto algo");

            }
            catch (Exception ex)
            {
                return StatusCode(409);
            }

        }

        [HttpPut("{team_idfk}")]
        public void Put(int team_idfk, [FromBody] TeamHero teamHero)
        {
            teamHero.team_idfk = team_idfk;
            if (ModelState.IsValid)
                teamHeroRepository.Update(teamHero);
        }

        [HttpDelete("{hero_idfk}")]
        public void DeleteRelationship(int hero_idfk)
        {
            teamHeroRepository.DeleteRelationship(hero_idfk);
        }
    }
}