using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Repositories;
using AutoMapper;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.AuctionOperations.Application.QueryHandlers;

public class GetAllActiveAuctions : IQueryHandler<GetAllActiveAuctionsQuery, ServiceResult<List<Auction>>>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IMapper _mapper;

    public GetAllActiveAuctions(IAuctionRepository auctionRepository, IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<Auction>>> Handle(GetAllActiveAuctionsQuery query)
    {
        try
        {
            var auctions = await _auctionRepository.GetAllActiveAuctionsAsync();
            return ServiceResult<List<Auction>>.Success(auctions);
        }
        catch (Exception ex)
        {
            // Log the exception details and handle the error
            return ServiceResult<List<Auction>>.Failure("Failed to retrieve active auctions.");
        }
    }
}



public record struct GetAllActiveAuctionsQuery : IQuery<ServiceResult<List<Auction>>>
{
    // Currently no properties, but it's here to represent a specific querying intention
}
