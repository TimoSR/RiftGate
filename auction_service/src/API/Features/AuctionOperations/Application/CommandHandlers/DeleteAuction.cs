using System.ComponentModel.DataAnnotations;
using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.AuctionOperations.Application.CommandHandlers;

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
                _logger.LogWarning("Auction with ID {AuctionId} not found.", command.AuctionId);
                return ServiceResult.Failure($"Auction with ID {command.AuctionId} not found.");
            }

            await _auctionRepository.DeleteAsync(auction);

            _logger.LogInformation("Auction with ID {AuctionId} was successfully deleted.", command.AuctionId);
            return ServiceResult.Success("Auction deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete auction with ID {AuctionId} due to an unexpected error.", command.AuctionId);
            return ServiceResult.Failure("Failed to delete the auction due to an unexpected error.");
        }
    }
}

public record struct DeleteAuctionCommand(string AuctionId) : ICommand;

public record struct DeleteAuctionRequest : IRequest
{
    [Required(ErrorMessage = "Auction ID is required.")]
    [StringLength(24, ErrorMessage = "Auction ID must be a 24-character string.", MinimumLength = 24)]
    public string AuctionId { get; set; }
}
