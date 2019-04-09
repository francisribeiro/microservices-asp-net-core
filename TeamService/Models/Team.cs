using System;
using System.Collections.Generic;

namespace TeamService.Models
{
    public class Team
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public ICollection<Member> Members { get; set; }

        public Team()
        {
            Members = new List<Member>();
        }

        public Team(string name) : this()
        {
            Name = name;
        }

        public Team(string name, Guid id) : this(name)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
