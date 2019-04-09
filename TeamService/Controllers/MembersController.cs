using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeamService.LocationClient;
using TeamService.Models;
using TeamService.Persistence;

namespace TeamService.Controllers
{
    [Route("/teams/{teamId}/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILocationClient _locationClient;

        public MembersController(ITeamRepository teamRepository, ILocationClient locationClient)
        {
            _teamRepository = teamRepository;
            _locationClient = locationClient;
        }

        [HttpGet]
        public virtual IActionResult GetMembers(Guid teamID)
        {
            var team = _teamRepository.Get(teamID);

            if (team == null)
                return NotFound();

            return Ok(team.Members);
        }

        [HttpGet]
        [Route("/teams/{teamId}/[controller]/{memberId}")]
        public async virtual Task<IActionResult> GetMember(Guid teamID, Guid memberId)
        {
            var team = _teamRepository.Get(teamID);

            if (team == null)
                return NotFound();

            var query = team.Members.Where(m => m.ID == memberId);

            if (query.Count() < 1)
                return NotFound();

            Member member = query.First();

            return Ok(new LocatedMember
            {
                ID = member.ID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                LastLocation = await _locationClient.GetLatestForMember(member.ID)
            });
        }

        [HttpGet]
        [Route("/members/{memberId}/team")]
        public IActionResult GetMemberTeamId(Guid memberId)
        {
            var result = GetTeamIdForMember(memberId);

            if (result != Guid.Empty)
                return Ok(new { TeamID = result });

            return NotFound();
        }

        [HttpPut]
        [Route("/teams/{teamId}/[controller]/{memberId}")]
        public virtual IActionResult UpdateMember([FromBody]Member updatedMember, Guid teamID, Guid memberId)
        {
            var team = _teamRepository.Get(teamID);

            if (team == null)
                return NotFound();

            var query = team.Members.Where(m => m.ID == memberId);

            if (query.Count() < 1)
                return NotFound();

            team.Members.Remove(query.First());
            team.Members.Add(updatedMember);
            _teamRepository.Update(team);

            return Ok();
        }

        [HttpPost]
        public virtual IActionResult CreateMember([FromBody]Member newMember, Guid teamID)
        {
            var team = _teamRepository.Get(teamID);

            if (team == null)
                return NotFound();

            team.Members.Add(newMember);
            _teamRepository.Update(team);
            var teamMember = new { TeamID = team.ID, MemberID = newMember.ID };

            return Created($"/teams/{teamMember.TeamID}/[controller]/{teamMember.MemberID}", teamMember);
        }

        private Guid GetTeamIdForMember(Guid memberId)
        {
            foreach (var team in _teamRepository.List())
            {
                var member = team.Members.FirstOrDefault(m => m.ID == memberId);
                if (member != null)
                    return team.ID;
            }

            return Guid.Empty;
        }
    }
}
