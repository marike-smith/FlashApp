using Asp.Versioning;
using FlashApp.API.Constants;
using FlashApp.API.Controllers.Base;
using FlashApp.Application.SensitiveWords.Add;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlashApp.API.Controllers.SensitiveWords;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class SensitiveWordController(ISender sender) : BaseController(sender)
{
    private readonly ISender _sender = sender;

    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddWordRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}