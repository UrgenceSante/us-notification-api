namespace UsNotificationApi.Domain.WorkSessions;
public class SessionBreak
{
    public int Id { get; set; }
    public DateTime BreakStart { get; set; }
    public DateTime BreakEnd { get; set; }
    public int BreakDuration { get; set; }
}