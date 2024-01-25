using System.ComponentModel.DataAnnotations;
using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;
using Infrastructure.ValidationAttributes;

namespace API.Features.AuctionOperations.Application.CommandHandlers.DeleteAuction;

public class DeleteAuction : ICommandHandler<DeleteAuctionCommand>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ILogger<DeleteAuction> _logger;

    public DeleteAuction(IAuctionRepository auctionRepository, ILogger<DeleteAuction> logger)
    {
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResult> Handle(DeleteAuctionCommand command)
    {
        try
        {
            var auction = await _auctionRepository.GetByIdAsync(command.AuctionId);
            if (auction == null)
            {
                _logger.LogWarning("Auction with ID {AuctionID} not found.", command.AuctionId);
                return ServiceResult.Failure($"Auction with ID {command.AuctionId} not found.");
            }

            await _auctionRepository.DeleteAsync(auction);

            _logger.LogInformation("Auction with ID {AuctionID} was successfully deleted.", command.AuctionId);
            return ServiceResult.Success("Auction deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete auction with ID {AuctionID} due to an unexpected error.", command.AuctionId);
            return ServiceResult.Failure("Failed to delete the auction due to an unexpected error.");
        }
    }
}

public record struct DeleteAuctionCommand(Guid RequestId, string AuctionId) : ICommand;

public record struct DeleteAuctionRequest : IRequest
{
    public Guid RequestId { get; set; }
    
    [Required(ErrorMessage = "Auction ID is required.")]
    [HexString(24)]
    public string AuctionId { get; set; }
}
