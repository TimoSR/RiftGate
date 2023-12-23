using System.ComponentModel.DataAnnotations;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.ApplicationLayer.ApplicationServices;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

public class PlaceBid : ICommandHandler<PlaceBidCommand>
{
    private readonly IAuctionRepository _auctionRepository;

    public PlaceBid(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    public async Task Handle(PlaceBidCommand command)
    {
        var auction = await _auctionRepository.GetByIdAsync(command.AuctionId);
        if (auction == null)
            throw new KeyNotFoundException($"Auction with ID {command.AuctionId} not found.");

        auction.PlaceBid(command.Bid);
        await _auctionRepository.UpdateAsync(auction);
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