// using API.Features.AuctionOperations.Application.QueryHandlers.DTO;
// using API.Features.AuctionOperations.Domain.Repositories;
// using AutoMapper;
// using CodingPatterns.ApplicationLayer.ApplicationServices;
// using CodingPatterns.ApplicationLayer.ServiceResultPattern;
// using CodingPatterns.ApplicationLayer.ServiceResultPattern._Enums;
//
// namespace API.Features.AuctionOperations.Application.QueryHandlers;
//
// public class SearchAuctions : IQueryHandler<SearchAuctionsQuery, ServiceResult<List<ActiveAuctionDto>>>
// {
//     
//     private readonly IAuctionRepository _auctionRepository;
//     private readonly IMapper _mapper;
//
//     public SearchAuctions(IAuctionRepository auctionRepository, IMapper mapper)
//     {
//         _auctionRepository = auctionRepository;
//         _mapper = mapper;
//     }
//     
//     public async Task<ServiceResult<List<ActiveAuctionDto>>> Handle(SearchAuctionsQuery query)
//     {
//         try
//         {
//             var auctions = await _auctionRepository.SearchAuctionsAsync(query.Name, query.Category, query.Group, query.Type, query.Rarity);
//             var auctionDtos = _mapper.Map<List<ActiveAuctionDto>>(auctions);
//             return ServiceResult<List<ActiveAuctionDto>>.Success(auctionDtos);
//         }
//         catch (Exception ex)
//         {
//             // Handle the exception
//             return ServiceResult<List<ActiveAuctionDto>>.Failure(new List<string> { "Failed to search auctions." }, ServiceErrorType.InternalError);
//         }
//     }
// }
//
// public record struct SearchAuctionsQuery : IQuery<ServiceResult<List<ActiveAuctionDto>>>
// {
//     public string? Name { get; set; }
//     public string? Category { get; set; }
//     public string? Group { get; set; }
//     public string? Type { get; set; }
//     public string? Rarity { get; set; }
//
//     public SearchAuctionsQuery(string? name, string? category, string? group, string? type, string? rarity)
//     {
//         Name = name;
//         Category = category;
//         Group = group;
//         Type = type;
//         Rarity = rarity;
//     }
// }
