using Asp.Versioning;
using FlashApp.API.Constants;
using FlashApp.API.Controllers.Base;
using FlashApp.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlashApp.API.Controllers.Users
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController(ISender sender) : BaseController(sender)
    {
        private readonly ISender _sender = sender;

        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.EmailAddress,
                request.Password);

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
