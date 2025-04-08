using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.Domain.DTO_s.Responses;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Core.Services
{
    public class AuctionsService:IAuctionsService
    {
        private readonly IAuctionsRepo _auctionsRepo;
        private readonly ILogger<AuctionsService> _logger;


        public AuctionsService(IAuctionsRepo auctionsRepo,ILogger<AuctionsService> logger)
        {
            _auctionsRepo = auctionsRepo;
            _logger = logger;
        }

        public async Task<OperationResult> CreateAuction(CreateAuction_Req auction_Req)
        {
            var timeConditions = CheckTimeConditions(auction_Req.OpeningTime, auction_Req.ClosingTime);

            if(!timeConditions)
            {
                return OperationResult.Failure("Did not meet the time conditions");
            }

            var newAuction = new Auctions()
            {
                Title = auction_Req.Title,
                Description = auction_Req.Description,
                StartingPrice = auction_Req.StartingPrice,
                OpeningTime = auction_Req.OpeningTime,
                ClosingTime = auction_Req.ClosingTime,
                CategoryId = auction_Req.CategoryId,
                UserId = auction_Req.UserId,
               
            };

            var result = await _auctionsRepo.CreateAuction(newAuction);

            return result;
        }

        public async Task<OperationResult> DeleteAuctions(int auctionId, int userId)
        {
            var result = await _auctionsRepo.DeleteAuctions(auctionId, userId);

            return result;
        }

        public async Task<OperationResult> GetAllAuctions()
        {
            var theAuctions = await _auctionsRepo.GetAllAuctions();

            if(theAuctions == null)
            {
                return OperationResult.Failure("auction not found.");
            }

            var auctionsToReturn = theAuctions.Select(item =>
            new AuctionDTO()
            {
                AuctionId = item.AuctionId,
                Title = item.Title,
                Description = item.Description,
                StartingPrice = item.StartingPrice,
                OpeningTime = item.OpeningTime,
                ClosingTime = item.ClosingTime,
                HighestBid = item.HighestBid,
                CategoryId = item.CategoryId,
                UserId = item.UserId,
            }).ToList().AsReadOnly();

            return OperationResult.Success(auctionsToReturn,"Auction found!");


        }

        public async Task<OperationResult> GetAuctionsWithId(int auctionId)
        {
            var theAuctions = await _auctionsRepo.GetAuctionsWithId(auctionId);

            if(theAuctions == null)
            {
                return OperationResult.Failure("Auction not found.");
            }

            var auctionToReturn = new AuctionDTO()
            {
                AuctionId = theAuctions.AuctionId,
                Title = theAuctions.Title,
                Description = theAuctions.Description,
                StartingPrice = theAuctions.StartingPrice,
                OpeningTime = theAuctions.OpeningTime,
                ClosingTime = theAuctions.ClosingTime,
                HighestBid= theAuctions.HighestBid,
                CategoryId = theAuctions.CategoryId,
                UserId = theAuctions.UserId,
            };

            return OperationResult.Success(auctionToReturn,"Auction found!");
        }

        public async Task<OperationResult> UpdateAuction(UpdateAuction_Req updateAuction_Req)
        {
           
            var auctionToUpdate =await _auctionsRepo.GetAuctionsWithId(updateAuction_Req.AuctionId);

            if(auctionToUpdate == null)
            {
                return OperationResult.Failure($"Auction to update with id:{updateAuction_Req.AuctionId} not found..");

            }

            if (updateAuction_Req.ClosingTime.HasValue)
            {
                if (!CheckTimeConditions(auctionToUpdate.OpeningTime, updateAuction_Req.ClosingTime.Value))
                {
                    return OperationResult.Failure("Time conditions not met for auction update.");
                }

                auctionToUpdate.ClosingTime = updateAuction_Req.ClosingTime.Value;
            }

           

            auctionToUpdate.Title = updateAuction_Req.Title ?? auctionToUpdate.Title;
            auctionToUpdate.Description = updateAuction_Req.Description ?? auctionToUpdate.Description;
            auctionToUpdate.StartingPrice = updateAuction_Req.StartingPrice ?? auctionToUpdate.StartingPrice;
            auctionToUpdate.CategoryId = updateAuction_Req.CategoryId ?? auctionToUpdate.CategoryId;

            var result = await _auctionsRepo.UpdateAuction(auctionToUpdate);

            return result;
        }

        private bool CheckTimeConditions(DateTime openingTime,DateTime closingTime)
        {
            var now = DateTime.UtcNow;
            var minDuration = TimeSpan.FromMinutes(30);
            var maxDuration = TimeSpan.FromDays(14);
            var openUtc = TimeZoneInfo.ConvertTimeToUtc(openingTime);
            var closeUtc = TimeZoneInfo.ConvertTimeToUtc(closingTime);
            var duration = closeUtc - openUtc;
        

            if (openUtc <= now)
            {
                _logger.LogDebug("Opening time must be in the future");
                return false;
            }

            if (closeUtc < openUtc)
            {
                _logger.LogDebug("Closing time must be after opening time.");
                return false;
            }

            if (duration < minDuration)
            {
                _logger.LogDebug("Auction must be open for at least 30 minutes.");
                return false;
            }

            if (duration > maxDuration)
            {
                _logger.LogDebug("Auction duration cannot exceed 14 days.");
                return false;
            }

            return true;

        }
    }
}
