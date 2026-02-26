namespace UsNotificationApi.Application.Common;

using Microsoft.AspNetCore.Mvc;

public class HttpPresenter<T> : IResultPresenter<T>
{
    public ActionResult PresentError(string errorMsg)
    {
        return new ObjectResult(errorMsg) { StatusCode = 500 };
    }

    public ActionResult PresentSuccess(T entity)
    {
        return new OkObjectResult(entity);
    }
}