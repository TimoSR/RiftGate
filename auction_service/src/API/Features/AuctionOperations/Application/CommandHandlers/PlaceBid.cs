using System.ComponentModel.DataAnnotations;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

public class PlaceBid : ICommandHandler<PlaceBidCommand>
{
    private readonly IAuctionRepository _auctionRepository;

    public PlaceBid(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    public async Task<ServiceResult> Handle(PlaceBidCommand command)
    {
        try
        {
            var auction = await _auctionRepository.GetByIdAsync(command.AuctionId);
            if (auction == null)
                return ServiceResult.Failure($"Auction with ID {command.AuctionId} not found.");

            // Additional logic can be implemented to check if the bid can be placed
            // For example, checking if the bid amount is higher than the current highest bid
            // and whether the auction is still active.

            auction.PlaceBid(command.Bid);
            await _auctionRepository.UpdateAsync(auction);

            return ServiceResult.Success("Bid placed successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return ServiceResult.Failure("Failed to place bid due to an unexpected error.");
        }
    }
}

// Requests have the responsibility to fail fast and be the endpoint contract
// !ModelState.IsValid in Controller

public record struct PlaceBidRequest : IRequest
{
    [Required]
    public Guid RequestId { get; set; }
    [Required(ErrorMessage = "{0} is required", AllowEmptyStrings = false)]
    public string AuctionId { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    public Bid Bid { get; set; }
}

// For Internal Concerns

public record struct PlaceBidCommand(Guid RequestId, string AuctionId, Bid Bid) : ICommand;