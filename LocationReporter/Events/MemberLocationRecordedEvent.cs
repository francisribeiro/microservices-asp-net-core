using Newtonsoft.Json;
using System;

namespace LocationReporter.Events
{
    public class MemberLocationRecordedEvent
    {
        public string Origin { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid MemberID { get; set; }
        public long RecordedTime { get; set; }
        public Guid ReportID { get; set; }
        public Guid TeamID { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
