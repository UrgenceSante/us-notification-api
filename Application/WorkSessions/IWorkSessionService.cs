using UsNotificationApi.Domain.WorkSessions;

namespace UsNotificationApi.Application.WorkSessions;

public interface IWorkSessionService
{
    WorkSession? GetWorkSessionByUserId(string userId);
    WorkSession? UpdateWorkSessionStatus(int sessionId, string status);
}
