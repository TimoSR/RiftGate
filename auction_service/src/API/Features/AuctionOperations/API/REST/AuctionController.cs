using API.Features.AuctionOperations.Application.CommandHandlers;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using HotChocolate.Authorization;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;

namespace API.Features.AuctionOperations.API.REST;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerDoc("Auction")]
[ApiVersion("1.0")]
[Authorize]
public class AuctionController : ControllerBase
{
    private readonly ICommandHandler<CompleteAuctionCommand> _completeAuctionHandler;
    private readonly ILogger<AuctionController> _logger;

    public AuctionController(ICommandHandler<CompleteAuctionCommand> completeAuctionHandler, ILogger<AuctionController> logger)
    {
        _completeAuctionHandler = completeAuctionHandler;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("CompleteAuction")]
    public async Task<IActionResult> CompleteAuction([FromBody] CompleteAuctionCommand command)
    {
        var result = await _completeAuctionHandler.Handle(command);

        if (result.IsSuccess)
        {
            return Ok(result.Messages);
        }
        else
        {
            _logger.LogError(result.Messages.ToString());
            return BadRequest(result.Messages);
        }
    }
}
