using API.Features.AuctionListing.Domain.AuctionAggregates.Repositories;
using AutoMapper;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.AuctionListing.Application.QueryHandlers;

public class GetActiveAuctions : IQueryHandler<GetActiveAuctionsQuery, ServiceResult<List<AuctionDto>>>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IMapper _mapper;

    public GetActiveAuctions(IAuctionRepository auctionRepository, IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<AuctionDto>>> Handle(GetActiveAuctionsQuery query)
    {
        try
        {
            var auctions = await _auctionRepository.GetAllActiveAuctionsAsync();
            var auctionDtos = _mapper.Map<List<AuctionDto>>(auctions);
            return ServiceResult<List<AuctionDto>>.Success(auctionDtos);
        }
        catch (Exception ex)
        {
            // Log the exception details and handle the error
            return ServiceResult<List<AuctionDto>>.Failure("Failed to retrieve active auctions.");
        }
    }
}

public class AuctionDto
{
    public string AuctionId { get; set; }
    public string Title { get; set; }
}

public class GetActiveAuctionsQuery : IQuery<ServiceResult<List<AuctionDto>>>
{
    // Currently no properties, but it's here to represent a specific querying intention
}
