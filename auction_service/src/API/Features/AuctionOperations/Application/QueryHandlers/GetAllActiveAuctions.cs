using System.Globalization;
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
            var collection = _auctionRepository.GetAuctionCollection();
            
            // Build your filter based on the Auction fields
            var filter = Builders<Auction>.Filter.Eq(a => a.IsActive, true);
            
            // Project the Auction fields directly to AuctionDTO
            var auctionDTOs = await collection
                .Find(filter)
                .Project(auction => new AuctionDTO
                {
                    Id = auction.Id.ToString(), // Assuming Id is an ObjectId
                    StartTime = auction.StartTime,
                    EstimatedEndTime = auction.EstimatedEndTime,
                    IsActive = auction.IsActive,
                    BuyoutAmount = auction.BuyoutAmount != null ? auction.BuyoutAmount.Value : 0
                })
                .ToListAsync();
            
            
            //var auctionDTOs = _mapper.Map<List<AuctionDTO>>(auctions);
            return ServiceResult<List<AuctionDTO>>.Success(auctionDTOs);
        }
        catch (Exception ex)
        {
            // Log the exception details and handle the error
            return ServiceResult<List<AuctionDTO>>.Failure("Failed to retrieve active auctions.");
        }
    }
}

public class ActiveAuctionsMappingProfile : Profile
{
    public ActiveAuctionsMappingProfile()
    {
        CreateMap<Auction, AuctionDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EstimatedEndTime, opt => opt.MapFrom(src => src.EstimatedEndTime))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.BuyoutAmount, opt => opt.MapFrom(src => src.BuyoutAmount.Value));
    }
}


public record struct AuctionDTO
{
    public string Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EstimatedEndTime { get; init; }
    public bool IsActive { get; init; }
    public Decimal BuyoutAmount { get; init; } // Assuming Price is a custom type
}

public record struct GetAllActiveAuctionsQuery : IQuery<ServiceResult<List<AuctionDTO>>>
{
    // Currently no properties, but it's here to represent a specific querying intention
}
