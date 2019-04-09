using Microsoft.AspNetCore.Mvc;
using System;
using TeamService.Models;
using TeamService.Persistence;

namespace TeamService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        public TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        public virtual IActionResult GetAllTeams()
        {
            return Ok(_teamRepository.List());
        }

        [HttpGet("{id}")]
        public IActionResult GetTeam(Guid id)
        {
            var team = _teamRepository.Get(id);

            if (team != null) // I HATE NULLS, MUST FIXERATE THIS.	 
                return Ok(team);

            return NotFound();
        }

        [HttpPost]
        public virtual IActionResult CreateTeam([FromBody]Team newTeam)
        {
            _teamRepository.Add(newTeam);

            //TODO: add test that asserts result is a 201 pointing to URL of the created team.
            //TODO: teams need IDs
            //TODO: return created at route to point to team details			
            return Created($"/teams/{newTeam.ID}", newTeam);
        }

        [HttpPut("{id}")]
        public virtual IActionResult UpdateTeam([FromBody]Team team, Guid id)
        {
            team.ID = id;

            if (_teamRepository.Update(team) == null)
                return NotFound();

            return Ok(team);
        }

        [HttpDelete("{id}")]
        public virtual IActionResult DeleteTeam(Guid id)
        {
            var team = _teamRepository.Delete(id);

            if (team == null)
                return NotFound();

            return Ok(team.ID);
        }
    }
}
