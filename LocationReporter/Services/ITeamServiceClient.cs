using System;

namespace LocationReporter.Services
{
    public interface ITeamServiceClient
    {
        Guid GetTeamForMember(Guid memberId);
    }
}
