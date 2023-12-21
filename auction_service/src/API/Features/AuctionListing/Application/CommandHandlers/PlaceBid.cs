using API.Features.AuctionListing.Domain.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AuctionAggregates.Repositories;
using CodingPatterns.ApplicationLayer.ApplicationServices;

namespace API.Features.AuctionListing.Application.CommandHandlers;

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

// If ever working with more team members introducing Request for the controllers could be an idea.
// For personal projects I think it is fine going Commands and Queries. 

public record struct PlaceBidCommand : ICommand
{
    public string AuctionId { get; }
    public Bid Bid { get; }

    public PlaceBidCommand(string auctionId, Bid bid)
    {
        AuctionId = auctionId;
        Bid = bid;
    }
}