using API.Features.AuctionOperations.Application.DTO;
using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Repositories;
using AutoMapper;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;
using MongoDB.Driver;

namespace API.Features.AuctionOperations.Application.QueryHandlers;

public class GetAllActiveAuctions : IQueryHandler<GetAllActiveAuctionsQuery, ServiceResult<List<AuctionDTO>>>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IMapper _mapper;

    public GetAllActiveAuctions(IAuctionRepository auctionRepository, IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
    }
    
    public async Task<ServiceResult<List<AuctionDTO>>> Handle(GetAllActiveAuctionsQuery query)
    {
        try
        {
            var collection = _auctionRepository.GetAuctionCollection();

            // Build your filter based on the Auction fields
            var filter = Builders<Auction>.Filter.Eq(a => a.IsActive, true);

            // Retrieve a list of Auctions
            var auctions = await collection.Find(filter).ToListAsync();

            // Use AutoMapper to map the list of Auctions to a list of ActiveAuctionDTOs
            var activeAuctionDTOs = _mapper.Map<List<AuctionDTO>>(auctions);

            return ServiceResult<List<AuctionDTO>>.Success(activeAuctionDTOs);
        }
        catch (Exception ex)
        {
            // Log the exception details and handle the error
            return ServiceResult<List<AuctionDTO>>.Failure("Failed to retrieve active auctions.");
        }
    }

    // public async Task<ServiceResult<List<AuctionDTO>>> Handle(GetAllActiveAuctionsQuery query)
    // {
    //     try
    //     {
    //         var collection = _auctionRepository.GetAuctionCollection();
    //         
    //         // Build your filter based on the Auction fields
    //         var filter = Builders<Auction>.Filter.Eq(a => a.IsActive, true);
    //         
    //         // Project the Auction fields directly to AuctionDTO
    //         var activeAuctionDTOs = await collection
    //             .Find(filter)
    //             .Project(auction => new AuctionDTO
    //             {
    //                 Id = auction.Id.ToString(), // Assuming Id is an ObjectId
    //                 StartTime = auction.StartTime,
    //                 EstimatedEndTime = auction.EstimatedEndTime,
    //                 IsActive = auction.IsActive,
    //                 BuyoutAmount = auction.BuyoutAmount != null ? auction.BuyoutAmount.Value : 0
    //             })
    //             .ToListAsync();
    //         
    //         
    //         //var auctionDTOs = _mapper.Map<List<AuctionDTO>>(auctions);
    //         return ServiceResult<List<AuctionDTO>>.Success(activeAuctionDTOs);
    //     }
    //     catch (Exception ex)
    //     {
    //         // Log the exception details and handle the error
    //         return ServiceResult<List<AuctionDTO>>.Failure("Failed to retrieve active auctions.");
    //     }
    // }
}

public record struct GetAllActiveAuctionsQuery : IQuery<ServiceResult<List<AuctionDTO>>>
{
    // Currently no properties, but it's here to represent a specific querying intention
}

