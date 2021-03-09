using dapper_heroes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_heroes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly HeroRepository heroRepository;

        public HeroController()
        {
            heroRepository = new HeroRepository();
        }

        [HttpGet]
        public IEnumerable<Hero> Get()
        {
            return heroRepository.GetAll();
        }

        [HttpGet("{id}")]
        public Hero Get(int id)
        {
            return heroRepository.GetById(id);
        }
        
        [HttpGet("powerfull")]
        //[Route("Powerfull")]
        //api/hero/powerfull
        public Hero GetPoderoso()
        {
            var resp = heroRepository.GetPowerfull();
            return resp;
        }
        

        [HttpPost]
        public IActionResult Post([FromBody] Hero hero)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newHero = heroRepository.Add(hero);
                    return Ok(newHero);
                }

                return BadRequest("falto algo");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Hero hero)
        {
            hero.id = id;
            if (ModelState.IsValid)
                heroRepository.Update(hero);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            heroRepository.Delete(id);
        }
    }

}
