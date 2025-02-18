using Asp.Versioning;
using FlashApp.API.Constants;
using FlashApp.API.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashApp.API.Controllers.Auth
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController(ISender sender) : BaseController(sender)
    {
        [Authorize]
        [HttpGet("apikey")]
        public IActionResult GetWithApiKey()
        {
            return Ok(new { message = "Access granted via API Key." });
        }
    }
}
