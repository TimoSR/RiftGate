using System.ComponentModel.DataAnnotations;
using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Repositories;
using API.Features.AuctionOperations.Domain.ValueObjects;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

public class CreateBuyoutAuction : ICommandHandler<CreateBuyoutAuctionCommand>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ILogger<CreateBuyoutAuction> _logger;

    public CreateBuyoutAuction(IAuctionRepository auctionRepository, ILogger<CreateBuyoutAuction> logger)
    {
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResult> Handle(CreateBuyoutAuctionCommand command)
    {
        try
        {
            // Create a new buyout auction
            var buyoutAuction = new BuyoutAuction(
                command.SellerId, 
                command.Item, 
                command.AuctionLength, 
                command.BuyoutAmount);

            // Persist the new auction to the repository
            await _auctionRepository.InsertAsync(buyoutAuction);

            _logger.LogInformation("Created a new buyout auction with ID {AuctionId}", buyoutAuction.Id);
            return ServiceResult.Success("Buyout auction created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating buyout auction");
            return ServiceResult.Failure("Failed to create buyout auction.");
        }
    }
}

public record struct CreateBuyoutAuctionCommand(string SellerId, Item Item, AuctionLength AuctionLength, Price BuyoutAmount) : ICommand;

public record struct CreateBuyoutAuctionRequest : IRequest
{
    [Required(ErrorMessage = "Seller ID is required.")]
    [StringLength(24, ErrorMessage = "Seller ID must be a 24-character string.", MinimumLength = 24)]
    public string SellerId { get; set; }

    [Required(ErrorMessage = "Item details are required.")]
    public Item Item { get; set; }

    [Required(ErrorMessage = "Auction length is required.")]
    public AuctionLength AuctionLength { get; set; }

    [Required(ErrorMessage = "Buyout amount is required.")]
    public Price BuyoutAmount { get; set; }
}


