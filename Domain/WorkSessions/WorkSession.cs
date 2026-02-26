namespace UsNotificationApi.Domain.WorkSessions;
public class WorkSession
{
    public int Id { get; set; }
    public required string KcUserId { get; set; }
    public required DateTime SessionStart { get; set; }
    public required DateTime SessionEnd { get; set; }
    public required List<SessionBreak> Breaks { get; set; }
    public required string SessionStatus { get; set; }
}