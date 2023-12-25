using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.ValueObjects;
using AutoMapper;

namespace API.Features.AuctionOperations.Application.DTO;

public record struct AuctionDTO
{
    public string Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EstimatedEndTime { get; init; }
    public bool IsActive { get; init; }
    public decimal? BuyoutAmount { get; init; } // Assuming Price has a decimal Value
    public List<BidDTO> Bids { get; init; } // Assuming you have a BidDTO
    public AuctionLengthDTO AuctionLength { get; init; } // Assuming you have an AuctionLengthDTO
    public ItemDTO Item { get; init; } // Assuming you have an ItemDTO
    public string SellerId { get; init; }
}

public record struct BidDTO
{
    public string Id { get; init; }
    public decimal BidAmount { get; init; }
    public string BidderId { get; init; }
    public DateTime TimeStamp { get; init; }
}

public record struct AuctionLengthDTO
{
    public int Value { get; init; }
}

public record struct ItemDTO
{
    public string ItemId { get; init; }
    public string Name { get; init; }
    public string Category { get; init; }
    public string Group { get; init; }
    public string Type { get; init; }
    public string Rarity { get; init; }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map Auction to AuctionDTO
        CreateMap<Auction, AuctionDTO>()
            .ForMember(dest => dest.BuyoutAmount, opt => opt.MapFrom(src => src.BuyoutAmount.Value))
            .ForMember(dest => dest.Bids, opt => opt.MapFrom(src => src.Bids))
            .ForMember(dest => dest.AuctionLength, opt => opt.MapFrom(src => new AuctionLengthDTO { Value = src.AuctionLength.Value }))
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item)) // Make sure Item is mapped correctly
            .ForMember(dest => dest.SellerId, opt => opt.MapFrom(src => src.SellerId));

        // Map Price to decimal
        CreateMap<Price, decimal>().ConvertUsing(src => src.Value);

        // Map Bid to BidDTO
        CreateMap<Bid, BidDTO>()
            .ForMember(dest => dest.BidAmount, opt => opt.MapFrom(src => src.BidAmount.Value))
            .ForMember(dest => dest.BidderId, opt => opt.MapFrom(src => src.BidderId))
            .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp));

        // Map AuctionLength to AuctionLengthDTO
        CreateMap<AuctionLength, AuctionLengthDTO>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
        
        CreateMap<Item, ItemDTO>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.Group))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Rarity, opt => opt.MapFrom(src => src.Rarity));
    }
}