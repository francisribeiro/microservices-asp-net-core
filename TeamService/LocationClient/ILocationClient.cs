using System;
using System.Threading.Tasks;
using TeamService.Models;

namespace TeamService.LocationClient
{
    public interface ILocationClient
    {
        Task<LocationRecord> GetLatestForMember(Guid memberId);
        Task<LocationRecord> AddLocation(Guid memberId, LocationRecord locationRecord);
    }
}
