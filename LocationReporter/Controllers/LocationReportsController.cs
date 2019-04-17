using LocationReporter.Events;
using LocationReporter.Models;
using LocationReporter.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LocationReporter.Controllers
{
    [Route("/api/members/{memberId}/locationreports")]
    [ApiController]
    public class LocationReportsController : ControllerBase
    {
        private ICommandEventConverter _converter;
        private IEventEmitter _eventEmitter;
        private ITeamServiceClient _teamServiceClient;

        public LocationReportsController(
            ICommandEventConverter converter,
            IEventEmitter eventEmitter,
            ITeamServiceClient teamServiceClient)
        {
            _converter = converter;
            _eventEmitter = eventEmitter;
            _teamServiceClient = teamServiceClient;
        }

        [HttpPost]
        public ActionResult PostLocationReport(Guid memberId, [FromBody]LocationReport locationReport)
        {
            MemberLocationRecordedEvent locationRecordedEvent = _converter.CommandToEvent(locationReport);
            locationRecordedEvent.TeamID = _teamServiceClient.GetTeamForMember(locationReport.MemberID);
            _eventEmitter.EmitLocationRecordedEvent(locationRecordedEvent);

            return Created($"/api/members/{memberId}/locationreports/{locationReport.ReportID}", locationReport);
        }
    }
}