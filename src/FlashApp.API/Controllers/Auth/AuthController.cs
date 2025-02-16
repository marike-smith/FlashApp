using Asp.Versioning;
using FlashApp.API.Constants;
using FlashApp.API.Controllers.Base;
using FlashApp.Application.Users.GetLoggedInUser;
using FlashApp.Application.Users.LogInUser;
using FlashApp.Domain.Entities.Abstractions;
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
        [HttpGet("apikey")]
        [Authorize(AuthenticationSchemes = "ApiKeyAuth")]
        public IActionResult GetWithApiKey()
        {
            return Ok(new { message = "Access granted via API Key." });
        }
        
        [HttpGet("me")]
        [MapToApiVersion(ApiVersions.V1)]
        public async Task<IActionResult> GetLoggedInUserV1(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();

            Result<UserResponse> result = await Sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LogInUserCommand(
                request.Email,
                request.Password);

            Result<LogInUserCommandResponse> result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
