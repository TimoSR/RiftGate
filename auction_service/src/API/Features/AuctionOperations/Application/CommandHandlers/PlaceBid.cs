using System.ComponentModel.DataAnnotations;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;
using Infrastructure.ValidationAttributes;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

public class PlaceBid : ICommandHandler<PlaceBidCommand>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ILogger<PlaceBid> _logger;

    public PlaceBid(IAuctionRepository auctionRepository, ILogger<PlaceBid> logger)
    {
        _auctionRepository = auctionRepository;
        _logger = logger;
    }

    public async Task<ServiceResult> Handle(PlaceBidCommand command)
    {
        try
        {
            var auction = await _auctionRepository.GetByIdAsync(command.AuctionId);
            if (auction == null)
            {
                _logger.LogWarning("Auction with ID {AuctionId} not found.", command.AuctionId);
                return ServiceResult.Failure($"Auction with ID {command.AuctionId} not found.");
            }

            auction.PlaceBid(command.Bid);
            await _auctionRepository.UpdateAsync(auction);

            _logger.LogInformation("Bid placed successfully for Auction ID {AuctionId}.", command.AuctionId);
            return ServiceResult.Success("Bid placed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to place bid on auction with ID {AuctionId}.", command.AuctionId);
            return ServiceResult.Failure("Failed to place bid due to an unexpected error.");
        }
    }
}

// For Internal Concerns

public record struct PlaceBidCommand(Guid RequestId, string AuctionId, Bid Bid) : ICommand;

// Requests have the responsibility to fail fast and be the endpoint contract

public record struct PlaceBidRequest : IRequest
{
    [Required]
    public Guid RequestId { get; set; }
    [Required(ErrorMessage = "The {0} field is required", AllowEmptyStrings = false)]
    [HexString(24)]
    public string AuctionId { get; set; }
    [Required(ErrorMessage = "The {0} field is required")]
    public Bid Bid { get; set; }
}