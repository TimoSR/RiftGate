using API.Features.AuctionOperations.Domain.Repositories;
using API.Features.AuctionOperations.Domain.Services;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

public class CompleteAuction : ICommandHandler<CompleteAuctionCommand>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ITimeService _timeService;
    private readonly ILogger<CompleteAuction> _logger;
    private readonly IAuctionExpiryChecker _test;

    public CompleteAuction(IAuctionRepository auctionRepository, ITimeService timeService, ILogger<CompleteAuction> logger, IAuctionExpiryChecker test)
    {
        _auctionRepository = auctionRepository;
        _timeService = timeService;
        _logger = logger;
        _test = test;
    }

    public async Task<ServiceResult> Handle(CompleteAuctionCommand command)
    {
        try
        {
            var auction = await _auctionRepository.GetByIdAsync(command.AuctionId);
            if (auction == null)
            {
                _logger.LogWarning("Auction with ID {AuctionId} not found for completion.", command.AuctionId);
                return ServiceResult.Failure($"Auction with ID {command.AuctionId} not found.");
            }

            auction.CheckAndCompleteAuction(_timeService);

            if (!auction.IsActive)
            {
                await _auctionRepository.UpdateAsync(auction);
            }
            
            _logger.LogInformation("Auction with ID {AuctionId} completed successfully.", command.AuctionId);
            return ServiceResult.Success("Auction completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to complete auction with ID {AuctionId} due to an unexpected error.", command.AuctionId);
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