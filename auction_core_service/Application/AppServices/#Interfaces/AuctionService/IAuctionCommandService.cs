using _SharedKernel.Patterns.ResultPattern;
using Application.DTO.AuctionService;

namespace Application.AppServices._Interfaces.AuctionService;

public interface IAuctionCommandService
{
    Task<ServiceResult<int>> CreateAuctionAsync(CreateAuctionCommand command);
    Task<ServiceResult> PlaceBidAsync(PlaceBidCommand command);
    Task<ServiceResult> CancelAuctionAsync(CancelAuctionCommand command);
    Task<ServiceResult> EndAuctionAsync(EndAuctionCommand command);
    Task<ServiceResult> BuyNowAsync(BuyNowCommand command);
    //Accept Bid
}