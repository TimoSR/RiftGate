using API.Features.AuctionOperations.Application.QueryHandlers;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.AuctionOperations.API.REST;

public partial class AuctionController
{
    [HttpGet("all-active-auctions")]
    public async Task<IActionResult> GetAllActiveAuctions()
    {
        var query = new GetAllActiveAuctionsQuery();
        var result = await _getAllActiveAuctionsHandler.Handle(query);
        return Ok(result.Data);
    }
}