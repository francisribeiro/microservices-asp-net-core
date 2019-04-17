namespace LocationReporter.Events
{
    public interface IEventEmitter
    {
        void EmitLocationRecordedEvent(MemberLocationRecordedEvent locationRecordedEvent);
    }
}
