using API.Features.AuctionOperations.Domain.Repositories;
using API.Features.AuctionOperations.Domain.Services;
using CodingPatterns.ApplicationLayer.ApplicationServices;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

public class CompleteAuction : ICommandHandler<CompleteAuctionCommand>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ITimeService _timeService;

    public CompleteAuction(IAuctionRepository auctionRepository, ITimeService timeService)
    {
        _auctionRepository = auctionRepository;
        _timeService = timeService;
    }

    public async Task Handle(CompleteAuctionCommand command)
    {
        var auction = await _auctionRepository.GetByIdAsync(command.AuctionId);
        if (auction == null)
            throw new KeyNotFoundException($"Auction with ID {command.AuctionId} not found.");

        auction.CheckAndCompleteAuction(_timeService);
        await _auctionRepository.UpdateAsync(auction);
    }
}

public class CompleteAuctionCommand : ICommand
{
    public string AuctionId { get; }

    public CompleteAuctionCommand(string auctionId)
    {
        AuctionId = auctionId;
    }
}