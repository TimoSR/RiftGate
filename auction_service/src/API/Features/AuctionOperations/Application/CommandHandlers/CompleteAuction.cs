using API.Features.AuctionOperations.Domain.Repositories;
using API.Features.AuctionOperations.Domain.Services;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

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

    public async Task<ServiceResult> Handle(CompleteAuctionCommand command)
    {
        try
        {
            var auction = await _auctionRepository.GetByIdAsync(command.AuctionId);
            if (auction == null)
                return ServiceResult.Failure($"Auction with ID {command.AuctionId} not found.");

            auction.CheckAndCompleteAuction(_timeService);
            await _auctionRepository.UpdateAsync(auction);

            return ServiceResult.Success("Auction completed successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return ServiceResult.Failure("Failed to complete the auction due to an unexpected error.");
        }
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