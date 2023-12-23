using CodingPatterns.ApplicationLayer.ApplicationServices;

namespace API.Features.AuctionListing.Application.DTO;

public class AuctionDto : IDto
{
    public string AuctionId { get; set; }
    public string Title { get; set; }
}