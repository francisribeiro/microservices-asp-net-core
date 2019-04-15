using System;
using System.Collections.Generic;
using System.Linq;
using LocationService.Models;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Persistence
{
    public class LocationRecordRepository : ILocationRecordRepository
    {
        private readonly LocationDbContext _context;
        private readonly DbSet<LocationRecord> _dataset;

        public LocationRecordRepository(LocationDbContext context)
        {
            _context = context;
            _dataset = _context.Set<LocationRecord>();
        }

        public LocationRecord Add(LocationRecord locationRecord)
        {
            _context.Add(locationRecord);
            _context.SaveChanges();

            return locationRecord;
        }

        public ICollection<LocationRecord> AllForMember(Guid memberId)
        {
            return _dataset.Where(lr => lr.MemberID == memberId).OrderBy(lr => lr.Timestamp).ToList();
        }

        public LocationRecord Delete(Guid memberId, Guid recordId)
        {
            var locationRecord = Get(memberId, recordId);
            _context.Remove(locationRecord);
            _context.SaveChanges();

            return locationRecord;
        }

        public LocationRecord Get(Guid memberId, Guid recordId)
        {
            return _dataset.FirstOrDefault(lr => lr.MemberID == memberId && lr.ID == recordId);
        }

        public LocationRecord GetLatestForMember(Guid memberId)
        {
            return _dataset.Where(lr => lr.MemberID == memberId).OrderBy(lr => lr.Timestamp).Last();
        }

        public LocationRecord Update(LocationRecord locationRecord)
        {
            _context.Entry(locationRecord).State = EntityState.Modified;
            _context.SaveChanges();

            return locationRecord;
        }
    }
}
