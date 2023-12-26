using System.ComponentModel.DataAnnotations;
using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Repositories;
using API.Features.AuctionOperations.Domain.Services;
using API.Features.AuctionOperations.Domain.ValueObjects;
using AutoMapper;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;
using Infrastructure.ValidationAttributes;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

public class CreateBuyoutAuction : ICommandHandler<CreateBuyoutAuctionCommand>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ILogger<CreateBuyoutAuction> _logger;
    private readonly IMapper _mapper;
    private readonly ITimeService _timeService;

    public CreateBuyoutAuction(
        IAuctionRepository auctionRepository,
        ILogger<CreateBuyoutAuction> logger,
        IMapper mapper,
        ITimeService timeService)
    {
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _timeService = timeService ?? throw new ArgumentException(nameof(timeService));
    }

    public async Task<ServiceResult> Handle(CreateBuyoutAuctionCommand command)
    {
        // Use AutoMapper to map the command to domain entities
        var item = _mapper.Map<Item>(command);
        var auctionLength = new AuctionLength(command.AuctionLengthHours);
        var price = new Price(command.BuyoutAmount);

        // Create a new buyout auction
        var buyoutAuction = new BuyoutAuction(command.SellerId, item, auctionLength, price);
        
        buyoutAuction.StartAuction(_timeService);

        // Persist the new auction to the repository
        await _auctionRepository.InsertAsync(buyoutAuction);

        _logger.LogInformation(
            "Request {RequestId}: Created a new buyout auction with ID {AuctionID}", 
            command.RequestId, buyoutAuction.Id);
        
        return ServiceResult.Success("Buyout auction created successfully.");
    }
}

public record struct CreateBuyoutAuctionCommand(
    Guid RequestId,
    string SellerId, 
    string ItemId, 
    string ItemName, 
    string ItemCategory, 
    string ItemGroup, 
    string ItemType, 
    string ItemRarity, 
    int ItemQuantity,
    int AuctionLengthHours, 
    decimal BuyoutAmount
) : ICommand;

public record struct CreateBuyoutAuctionRequest : IRequest
{
    public Guid RequestId { get; set; }
    [Required(ErrorMessage = $"{nameof(SellerId)} is required.")]
    //[StringLength(24, ErrorMessage = $"{nameof(SellerId)} must be a 24-character string.", MinimumLength = 24)]
    [HexString(24)]
    public string SellerId { get; set; }

    // Item properties
    [Required(ErrorMessage = $"{nameof(ItemId)} is required.")]
    //[StringLength(24, ErrorMessage = $"{nameof(ItemId)} must be a 24-character string.", MinimumLength = 24)]
    [HexString(24)]
    public string ItemId { get; set; }

    [Required(ErrorMessage = "Item name is required.")]
    public string ItemName { get; set; }

    [Required(ErrorMessage = "Item category is required.")]
    public string ItemCategory { get; set; }

    [Required(ErrorMessage = "Item group is required.")]
    public string ItemGroup { get; set; }

    [Required(ErrorMessage = "Item type is required.")]
    public string ItemType { get; set; }

    [Required(ErrorMessage = "Item rarity is required.")]
    public string ItemRarity { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Item quantity must be at least 1.")]
    public int ItemQuantity { get; set; }

    // Auction length in hours
    [Required(ErrorMessage = "Auction length is required.")]
    [Range(12, 48, ErrorMessage = "Auction length must be 12, 24, or 48 hours.")]
    public int AuctionLengthHours { get; set; }

    // Price properties
    [Required(ErrorMessage = "Buyout amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Buyout amount must be greater than 0.")]
    public decimal BuyoutAmount { get; set; }
}

public class CreateBuyoutProfile : Profile
{
    public CreateBuyoutProfile()
    {
        CreateMap<CreateBuyoutAuctionRequest, CreateBuyoutAuctionCommand>();

        CreateMap<CreateBuyoutAuctionCommand, Item>()
            .ConstructUsing(src => 
                new Item(
                    src.ItemId, 
                    src.ItemName, 
                    src.ItemCategory, 
                    src.ItemGroup, 
                    src.ItemType, 
                    src.ItemRarity, 
                    src.ItemQuantity));
    }
}


