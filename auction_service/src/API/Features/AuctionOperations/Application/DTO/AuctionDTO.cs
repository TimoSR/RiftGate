using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.ValueObjects;
using AutoMapper;

namespace API.Features.AuctionOperations.Application.DTO;

public record struct AuctionDTO(
    string Id,
    bool IsActive,
    string SellerId,
    ItemDTO Item,
    decimal? BuyoutAmount,
    int AuctionLengthHours,
    DateTime StartTime,
    DateTime EstimatedEndTime,
    List<BidDTO> Bids
);

public record struct BidDTO(
    string Id,
    decimal BidAmount,
    string BidderId,
    DateTime TimeStamp
);

public record struct ItemDTO(
    string ItemId,
    string Name,
    string Category,
    string Group,
    string Type,
    string Rarity
);

public class AuctionProfile : Profile
{
    public AuctionProfile()
    {
        CreateMap<Auction, AuctionDTO>()
            .ForMember(dest => dest.BuyoutAmount, opt => opt.MapFrom(src => src.BuyoutAmount.Value))
            .ForMember(dest => dest.Bids, opt => opt.MapFrom(src => src.Bids))
            .ForMember(dest => dest.AuctionLengthHours, opt => opt.MapFrom(src => src.AuctionLengthHours.Value))
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item)); // Only specify mappings where names don't match exactly

        CreateMap<Price, decimal>().ConvertUsing(src => src.Value);
        CreateMap<AuctionLength, int>().ConvertUsing(src => src.Value);
        CreateMap<Bid, BidDTO>(); // AutoMapper handles matching properties by convention
        CreateMap<Item, ItemDTO>(); // Convention-based mapping
    }
}

