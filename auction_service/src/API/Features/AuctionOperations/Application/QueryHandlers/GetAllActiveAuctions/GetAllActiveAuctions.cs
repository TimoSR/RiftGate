using API.Features.AuctionOperations.Application.QueryHandlers.DTO;
using API.Features.AuctionOperations.Domain.Repositories;
using AutoMapper;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.AuctionOperations.Application.QueryHandlers.GetAllActiveAuctions;

public class GetAllActiveAuctions : IQueryHandler<GetAllActiveAuctionsQuery, ServiceResult<List<AuctionDTO>>>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllActiveAuctions> _logger;

    public GetAllActiveAuctions(
        IAuctionRepository auctionRepository, 
        IMapper mapper,
        ILogger<GetAllActiveAuctions> logger
        )
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<ServiceResult<List<AuctionDTO>>> Handle(GetAllActiveAuctionsQuery query)
    {
        var auctions = await _auctionRepository.GetAllActiveAuctionsAsync();
        
        var activeAuctionDTOs = _mapper.Map<List<AuctionDTO>>(auctions);

        return ServiceResult<List<AuctionDTO>>.Success(activeAuctionDTOs);
    }
}

public record struct GetAllActiveAuctionsQuery : IQuery<ServiceResult<List<AuctionDTO>>>
{
    // Currently no properties, but it's here to represent a specific querying intention
}

