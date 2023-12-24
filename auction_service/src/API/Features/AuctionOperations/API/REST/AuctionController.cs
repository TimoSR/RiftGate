using API.Features.AuctionOperations.Application.CommandHandlers;
using API.Features.AuctionOperations.Application.QueryHandlers;
using API.Features.AuctionOperations.Domain;
using AutoMapper;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;
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
    private readonly IMapper _mapper; 
    private readonly ICommandHandler<CompleteAuctionCommand> _completeAuctionHandler;
    private readonly ICommandHandler<CreateBuyoutAuctionCommand> _createBuyoutAuctionHandler;
    private readonly IQueryHandler<GetAllActiveAuctionsQuery, ServiceResult<List<AuctionDTO>>> _getAllActiveAuctionsHandler;
    
    public AuctionController(
        IMapper mapper,
        ICommandHandler<CompleteAuctionCommand> completeAuctionHandler,
        ICommandHandler<CreateBuyoutAuctionCommand> createBuyoutAuctionHandler,
        IQueryHandler<GetAllActiveAuctionsQuery, ServiceResult<List<AuctionDTO>>> getAllActiveAuctionsHandler)
    {
        _mapper = mapper;
        _completeAuctionHandler = completeAuctionHandler;
        _createBuyoutAuctionHandler = createBuyoutAuctionHandler;
        _getAllActiveAuctionsHandler = getAllActiveAuctionsHandler;
    }
    
    
    [AllowAnonymous]
    [HttpPost("create-buyout-auction")]
    public async Task<IActionResult> CreateBuyoutAuction([FromBody] CreateBuyoutAuctionRequest request)
    {
        var command = _mapper.Map<CreateBuyoutAuctionCommand>(request);

        var result = await _createBuyoutAuctionHandler.Handle(command);

        if (result.IsSuccess)
        {
            return Ok(result.Messages);
        }
        else
        {
            return BadRequest(result.Messages);
        }
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
    
    [AllowAnonymous]
    [HttpGet("all-active-auctions")]
    public async Task<IActionResult> GetAllActiveAuctions()
    {
        var query = new GetAllActiveAuctionsQuery();
        var result = await _getAllActiveAuctionsHandler.Handle(query);

        if (result.IsSuccess)
        {
            return Ok(result.Data);  // Assuming ServiceResult<T> has a Data property
        }
        else
        {
            return BadRequest(result.Messages);
        }
    }
}
