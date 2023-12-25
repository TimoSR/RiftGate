using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.ValueObjects;
using AutoMapper;

namespace API.Features.AuctionOperations.Application.DTO;

public record struct AuctionDTO(
    string Id,
    DateTime StartTime,
    DateTime EstimatedEndTime,
    bool IsActive,
    decimal? BuyoutAmount,
    List<BidDTO> Bids,
    AuctionLengthDTO AuctionLength,
    ItemDTO Item,
    string SellerId
);

public record struct BidDTO(
    string Id,
    decimal BidAmount,
    string BidderId,
    DateTime TimeStamp
);

public record struct AuctionLengthDTO(int Value);

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
            .ForMember(dest => dest.BuyoutAmount, opt => opt.MapFrom(src => src.BuyoutAmount))
            .ForMember(dest => dest.Bids, opt => opt.MapFrom(src => src.Bids))
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item));

        CreateMap<Price, decimal>().ConvertUsing(src => src.Value);
        CreateMap<Bid, BidDTO>(); // AutoMapper handles matching properties by convention
        CreateMap<AuctionLength, AuctionLengthDTO>(); // Convention-based mapping
        CreateMap<Item, ItemDTO>(); // Convention-based mapping
    }
}
