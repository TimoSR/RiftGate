using _SharedKernel.Patterns.ResultPattern;
using Application.DTO.AuctionService;

namespace Application.AppServices._Interfaces.AuctionService;

public interface IAuctionQueryService
{
    Task<ServiceResult<IEnumerable<AuctionDto>>> GetAllAuctionsAsync();
    Task<ServiceResult<AuctionDto>> GetAuctionByIdAsync(int auctionId);
    Task<ServiceResult<IEnumerable<BidDto>>> GetBidsByAuctionIdAsync(int auctionId);
}