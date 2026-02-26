using UsNotificationApi.Domain.WorkSessions;

namespace UsNotificationApi.Application.WorkSessions;

public class WorkSessionService : IWorkSessionService
{
    WorkSession defaultWorkSession = new()
    {
        Id = 12,
        KcUserId = "b198633e-0a12-4df6-85f8-c70cb62d6688", //kcId alex.delesse01@gmail.com
        Breaks = [],
        SessionStart = DateTime.UtcNow.AddHours(2),
        SessionEnd = DateTime.UtcNow.AddHours(10),
        SessionStatus = "Pending",
    };
    private static readonly string[] sourceArray = ["Pending", "Acknoledged", "Active", "End"];

    public WorkSession? GetWorkSessionByUserId(string userId)
    {
        if (defaultWorkSession.KcUserId != userId) return null;
        else return defaultWorkSession;
    }

    public WorkSession? UpdateWorkSessionStatus(int sessionId, string status)
    {
        if (defaultWorkSession.Id != sessionId) return null;
        if (!sourceArray.Contains(status)) return null;

        defaultWorkSession.SessionStatus = status;
        return defaultWorkSession;

    }


}