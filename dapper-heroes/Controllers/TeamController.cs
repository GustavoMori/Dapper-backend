﻿using dapper_heroes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dapper_heroes.Controllers
{
    [Route("api/team")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamRepository teamRepository;

        public TeamController()
        {
            teamRepository = new TeamRepository();
        }

        [HttpGet]
        public IEnumerable<Team> Get()
        {
            return teamRepository.GetAll();
        }

        [HttpGet("{id_team}")]
        public Team Get(int id_team)
        {
            return teamRepository.GetById(id_team);
        }

        [HttpGet("{id_team}/heroes")]
        public IEnumerable<Hero> GetListHeroes(int id_team)
        {
            return teamRepository.GetHeroByRelationship(id_team);
        }

        [HttpGet("{id}/teams")]
        public IEnumerable<Team> GetListTeams(int id)
        {
            return teamRepository.GetTeamByRelationship(id);
        }



        [HttpPost]
        public IActionResult Post([FromBody] Team team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTeam = teamRepository.Add(team);
                    return Ok(newTeam);
                }

                return BadRequest("falto algo");

            }
            catch (Exception ex)
            {
                return StatusCode(409);
            }

        }

        [HttpPost("relationship")]
        public async Task<IActionResult> AddHeroInTeam([FromBody] TeamHero teamHero)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await teamRepository.AddHeroInTeam(teamHero);
                    return Ok();
                }

                return BadRequest("falto algo");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpPost("relationship/kick")]
        public async Task<IActionResult> KickHero([FromBody] TeamHero teamHero)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await teamRepository.KickHero(teamHero);
                    return Ok();
                }

                return BadRequest("falto algo");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpPut("{id_team}")]
        public void Put(int id_team, [FromBody] Team team)
        {
            team.id_team = id_team;
            if (ModelState.IsValid)
                teamRepository.Update(team);
        }

        [HttpDelete("{id_team}")]
        public void Delete(int id_team)
        {
            teamRepository.Delete(id_team);
        }
    }
}
