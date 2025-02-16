using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlashApp.API.Controllers.Base
{
    public class BaseController : ControllerBase

    {
        protected readonly ISender Sender;

        protected BaseController(ISender sender)
        {
            Sender = sender;
        }
    }
}
