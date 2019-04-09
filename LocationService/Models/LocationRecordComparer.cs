using System.Collections.Generic;

namespace LocationService.Models
{
    public class LocationRecordComparer : Comparer<LocationRecord>
    {
        public override int Compare(LocationRecord x, LocationRecord y)
        {
            return x.Timestamp.CompareTo(y.Timestamp);
        }
    }
}
