using System.ComponentModel.DataAnnotations;
using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Repositories;
using API.Features.AuctionOperations.Domain.ValueObjects;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

public class CreateTraditionalAuction : ICommandHandler<CreateTradAuctionCommand>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ILogger<CreateTraditionalAuction> _logger;

    public CreateTraditionalAuction(IAuctionRepository auctionRepository, ILogger<CreateTraditionalAuction> logger)
    {
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResult> Handle(CreateTradAuctionCommand command)
    {
        try
        {
            // Create a new traditional auction
            var traditionalAuction = new TraditionalAuction(command.SellerId, command.Item, command.AuctionLength);

            // Persist the new auction to the repository
            await _auctionRepository.InsertAsync(traditionalAuction);

            _logger.LogInformation("Created a new traditional auction with ID {AuctionId}", traditionalAuction.Id);
            return ServiceResult.Success("Traditional auction created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating traditional auction");
            return ServiceResult.Failure("Failed to create traditional auction.");
        }
    }
}

public record struct CreateTradAuctionCommand(
    Guid? RequestID, 
    string SellerId, 
    Item Item, 
    AuctionLength AuctionLength) : ICommand;

public record struct CreateTraditionalAuctionRequest : IRequest
{
    public Guid? RequestID { get; set; }
    [Required(ErrorMessage = "Seller ID is required.")]
    [StringLength(24, ErrorMessage = "Seller ID must be a 24-character string.", MinimumLength = 24)]
    public string SellerId { get; set; }

    [Required(ErrorMessage = "Item details are required.")]
    public Item Item { get; set; }

    [Required(ErrorMessage = "Auction length is required.")]
    public AuctionLength AuctionLength { get; set; }    
}