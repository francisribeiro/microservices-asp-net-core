using LocationService.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LocationService.Controllers
{
    [Route("locations/{memberId}")]
    public class LocationRecordController : ControllerBase
    {
        private readonly ILocationRecordRepository locationRepository;

        public LocationRecordController(ILocationRecordRepository repository)
        {
            locationRepository = repository;
        }

        [HttpPost]
        public IActionResult AddLocation(Guid memberId, [FromBody]LocationRecord locationRecord)
        {
            locationRepository.Add(locationRecord);
            return Created($"/locations/{memberId}/{locationRecord.ID}", locationRecord);
        }

        [HttpGet]
        public IActionResult GetLocationsForMember(Guid memberId)
        {
            return Ok(locationRepository.AllForMember(memberId));
        }

        [HttpGet("latest")]
        public IActionResult GetLatestForMember(Guid memberId)
        {
            return Ok(locationRepository.GetLatestForMember(memberId));
        }
    }
}