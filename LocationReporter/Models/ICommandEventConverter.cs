using LocationReporter.Events;

namespace LocationReporter.Models
{
    public interface ICommandEventConverter
    {
        MemberLocationRecordedEvent CommandToEvent(LocationReport locationReport);
    }
}
