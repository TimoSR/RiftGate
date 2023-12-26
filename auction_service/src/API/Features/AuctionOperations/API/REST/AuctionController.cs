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
public partial class AuctionController : ControllerBase
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
}
