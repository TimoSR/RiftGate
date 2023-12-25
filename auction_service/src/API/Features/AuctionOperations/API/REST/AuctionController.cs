using API.Features.AuctionOperations.Application.CommandHandlers;
using API.Features.AuctionOperations.Application.DTO;
using API.Features.AuctionOperations.Application.QueryHandlers;
using AutoMapper;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.AuctionOperations.API.REST;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerDoc("Auction")]
[ApiVersion("1.0")]
public class AuctionController : ControllerBase
{
    private readonly IMapper _mapper; 
    private readonly ICommandHandler<CompleteAuctionCommand> _completeAuctionHandler;
    private readonly ICommandHandler<CreateBuyoutAuctionCommand> _createBuyoutAuctionHandler;
    private readonly ICommandHandler<PlaceBidCommand> _placeBidHandler;
        
    private readonly IQueryHandler<GetAllActiveAuctionsQuery, ServiceResult<List<AuctionDTO>>> _getAllActiveAuctionsHandler;
    
    public AuctionController(
        IMapper mapper,
        ICommandHandler<CompleteAuctionCommand> completeAuctionHandler,
        ICommandHandler<CreateBuyoutAuctionCommand> createBuyoutAuctionHandler,
        IQueryHandler<GetAllActiveAuctionsQuery, ServiceResult<List<AuctionDTO>>> getAllActiveAuctionsHandler,
        ICommandHandler<PlaceBidCommand> placeBidHandler)
    {
        _mapper = mapper;
        _completeAuctionHandler = completeAuctionHandler;
        _createBuyoutAuctionHandler = createBuyoutAuctionHandler;
        _getAllActiveAuctionsHandler = getAllActiveAuctionsHandler;
        _placeBidHandler = placeBidHandler;
    }
    
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

    [HttpPost("place-bid-on-auction")]
    public async Task<IActionResult> PlaceBidOnAuction([FromBody] PlaceBidRequest request)
    {
        var command = new PlaceBidCommand(
            request.RequestId, 
            request.AuctionId,
            request.BidderId,
            request.BidAmount);

        var result = await _placeBidHandler.Handle(command);

        if (result.IsSuccess)
        {
            return Ok(result.Messages);
        }

        return BadRequest(result.Messages);
    }

    [HttpPost("complete-auction")]
    public async Task<IActionResult> CompleteAuction([FromBody] CompleteAuctionRequest request)
    {
        var command = new CompleteAuctionCommand(request.RequestId, request.AuctionId);
        
        var result = await _completeAuctionHandler.Handle(command);

        if (result.IsSuccess)
        {
            return Ok(result.Messages);
        }

        return BadRequest(result.Messages);
    }
    
    [HttpGet("all-active-auctions")]
    public async Task<IActionResult> GetAllActiveAuctions()
    {
        var query = new GetAllActiveAuctionsQuery();
        var result = await _getAllActiveAuctionsHandler.Handle(query);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return BadRequest(result.Messages);
        }
    }
}
