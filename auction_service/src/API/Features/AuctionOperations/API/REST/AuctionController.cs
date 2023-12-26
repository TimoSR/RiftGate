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
//[Authorize]
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
        return Ok(result.Messages);
    }

    [HttpPost("place-bid-on-auction")]
    public async Task<IActionResult> PlaceBidOnAuction([FromBody] PlaceBidRequest request)
    {
        var command = _mapper.Map<PlaceBidCommand>(request);
        var result = await _placeBidHandler.Handle(command);
        return Ok(result.Messages);
    }

    [HttpPost("complete-auction")]
    public async Task<IActionResult> CompleteAuction([FromBody] CompleteAuctionRequest request)
    {
        var command = new CompleteAuctionCommand(request.RequestId, request.AuctionId);
        var result = await _completeAuctionHandler.Handle(command);
        return Ok(result.Messages);
    }
    
    [HttpGet("all-active-auctions")]
    public async Task<IActionResult> GetAllActiveAuctions()
    {
        var query = new GetAllActiveAuctionsQuery();
        var result = await _getAllActiveAuctionsHandler.Handle(query);
        return Ok(result.Data);
    }
}
