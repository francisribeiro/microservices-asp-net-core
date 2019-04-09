using System;
using System.Collections.Generic;
using System.Linq;
using TeamService.Models;

namespace TeamService.Persistence
{
    public class MemoryTeamRepository : ITeamRepository
    {
        protected static ICollection<Team> teams;

        public MemoryTeamRepository()
        {
            if (teams == null)
                teams = new List<Team>();
        }

        public MemoryTeamRepository(ICollection<Team> teams)
        {
            MemoryTeamRepository.teams = teams;
        }

        public IEnumerable<Team> List()
        {
            return teams;
        }

        public Team Get(Guid id)
        {
            return teams.FirstOrDefault(t => t.ID == id);
        }

        public Team Update(Team t)
        {
            var team = Delete(t.ID);

            if (team != null)
                team = Add(t);

            return team;
        }

        public Team Add(Team team)
        {
            teams.Add(team);
            return team;
        }

        public Team Delete(Guid id)
        {
            var query = teams.Where(t => t.ID == id);
            Team team = null;

            if (query.Count() > 0)
            {
                team = query.First();
                teams.Remove(team);
            }

            return team;
        }
    }
}
