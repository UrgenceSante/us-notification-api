namespace UsNotificationApi.Application.Common;

using Microsoft.AspNetCore.Mvc;

public interface IResultPresenter<T>
{
    ActionResult PresentSuccess(T entity);
    ActionResult PresentError(string errorMsg);
}