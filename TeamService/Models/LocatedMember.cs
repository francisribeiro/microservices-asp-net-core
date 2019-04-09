namespace TeamService.Models
{
    public class LocatedMember : Member
    {
        public LocationRecord LastLocation { get; set; }

        public LocatedMember() { }
    }
}
