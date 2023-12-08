using _SharedKernel.Patterns.ResultPattern;
using Application.DTO.AuctionService;

namespace Application.AppServices._Interfaces.AuctionService;

public interface IAuctionCommandService
{
    Task<ServiceResult<int>> CreateAuctionAsync(CreateAuctionCommand command);
    Task<ServiceResult> PlaceBidAsync(PlaceBidCommand command);
    Task<ServiceResult> CancelAuctionAsync(CancelAuctionCommand command);
}