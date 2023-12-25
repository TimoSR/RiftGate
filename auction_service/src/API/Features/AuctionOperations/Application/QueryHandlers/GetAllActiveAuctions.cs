using API.Features.AuctionOperations.Application.DTO;
using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Repositories;
using AutoMapper;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;
using MongoDB.Driver;

namespace API.Features.AuctionOperations.Application.QueryHandlers;

public class GetAllActiveAuctions : IQueryHandler<GetAllActiveAuctionsQuery, ServiceResult<List<AuctionDTO>>>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IMapper _mapper;

    public GetAllActiveAuctions(IAuctionRepository auctionRepository, IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
    }
    
    public async Task<ServiceResult<List<AuctionDTO>>> Handle(GetAllActiveAuctionsQuery query)
    {
        try
        {
            var auctions = await _auctionRepository.GetAllActiveAuctionsAsync();
            
            var activeAuctionDTOs = _mapper.Map<List<AuctionDTO>>(auctions);

            return ServiceResult<List<AuctionDTO>>.Success(activeAuctionDTOs);
        }
        catch (Exception ex)
        {
            // Log the exception details and handle the error
            return ServiceResult<List<AuctionDTO>>.Failure("Failed to retrieve active auctions.");
        }
    }
}

public record struct GetAllActiveAuctionsQuery : IQuery<ServiceResult<List<AuctionDTO>>>
{
    // Currently no properties, but it's here to represent a specific querying intention
}

