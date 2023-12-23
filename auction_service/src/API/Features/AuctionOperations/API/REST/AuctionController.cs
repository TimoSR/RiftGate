using API.Features.AuctionOperations.Application.CommandHandlers;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.AuctionOperations.API.REST;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerDoc("Auction")]
[ApiVersion("1.0")]
[Authorize]
public class AuctionController : ControllerBase
{
    private readonly ICommandHandler<CompleteAuctionCommand> _completeAuctionHandler;
    
    public AuctionController(ICommandHandler<CompleteAuctionCommand> completeAuctionHandler)
    {
        _completeAuctionHandler = completeAuctionHandler;
    }

    [AllowAnonymous]
    [HttpPost("CompleteAuction")]
    public async Task<IActionResult> CompleteAuction([FromBody] CompleteAuctionRequest request)
    {
        var command = new CompleteAuctionCommand(request.AuctionId);
        
        var result = await _completeAuctionHandler.Handle(command);

        if (result.IsSuccess)
        {
            return Ok(result.Messages);
        }

        return BadRequest(result.Messages);
    }
}
