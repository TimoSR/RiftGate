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
            var filter = Builders<Auction>.Filter.Eq(a => a.IsActive, true);
            var auctions = await collection.Find(filter).ToListAsync();
            var auctionDTOs = _mapper.Map<List<AuctionDTO>>(auctions);
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
        // Add mapping for Auction to AuctionDTO
        CreateMap<Auction, AuctionDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EstimatedEndTime, opt => opt.MapFrom(src => src.EstimatedEndTime))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        // Add other mappings for remaining properties as needed
    }
}


public class AuctionDTO
{
    public string Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EstimatedEndTime { get; set; }
    public bool IsActive { get; set; }
    // Add other properties that you need in your DTO
}

public record struct GetAllActiveAuctionsQuery : IQuery<ServiceResult<List<AuctionDTO>>>
{
    // Currently no properties, but it's here to represent a specific querying intention
}
