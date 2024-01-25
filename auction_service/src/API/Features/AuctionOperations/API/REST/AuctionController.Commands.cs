using API.Features.AuctionOperations.Application.CommandHandlers;
using API.Features.AuctionOperations.Application.CommandHandlers.CompleteAuction;
using API.Features.AuctionOperations.Application.CommandHandlers.CreateBuyoutAuction;
using API.Features.AuctionOperations.Application.CommandHandlers.PlaceBid;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.AuctionOperations.API.REST;

public partial class AuctionController
{
    [AllowAnonymous]
    [HttpPost("create-buyout-auction")]
    public async Task<IActionResult> CreateBuyoutAuction([FromBody] CreateBuyoutAuctionRequest request)
    {
        var command = _mapper.Map<CreateBuyoutAuctionCommand>(request);
        var result = await _createBuyoutAuctionHandler.Handle(command);
        return Ok(result.Messages);
    }

    [HttpPost("place-bid-on-auction")]
    public async Task<IActionResult> PlaceBidOnAuction([FromBody] PlaceBidRequest request)
    {
        var command = _mapper.Map<PlaceBidCommand>(request);
        var result = await _placeBidHandler.Handle(command);
        return Ok(result.Messages);
    }

    [HttpPost("complete-auction")]
    public async Task<IActionResult> CompleteAuction([FromBody] CompleteAuctionRequest request)
    {
        var command = new CompleteAuctionCommand(request.RequestId, request.AuctionId);
        var result = await _completeAuctionHandler.Handle(command);
        return Ok(result.Messages);
    }
}

