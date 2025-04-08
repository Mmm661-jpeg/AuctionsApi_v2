using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.Domain.DTO_s.Responses;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Core.Services
{
    public class BidsService:IBidsService
    {
        private readonly IBidsRepo _bidsRepo;

        public BidsService(IBidsRepo bidsRepo)
        {
            _bidsRepo = bidsRepo;
        }

        public async Task<OperationResult> MakeBid(MakeBid_Req makeBid_Req)
        {
            var bidToMake = new Bids()
            {
                UserId = makeBid_Req.UserId,
                AuctionId = makeBid_Req.AuctionId,
                BidAmount = makeBid_Req.BidAmount,
            };

            var result = await _bidsRepo.MakeBid(bidToMake);

            if (result.IsSuccces && result.Data is Bids successfulBid)
            {
                var dto = new BidDTO
                {
                    BidId = successfulBid.BidId,
                    AuctionId = successfulBid.AuctionId,
                    UserId = successfulBid.UserId,
                    BidAmount = successfulBid.BidAmount
                };

                return OperationResult.Success(dto, result.Message);
            }


            return result;
        }

        public async Task<OperationResult> RemoveBid(int userId, int bidId)
        {
            var result = await _bidsRepo.RemoveBid(userId, bidId);

            return result;
        }

        public async Task<OperationResult> ViewAllBidsWithID(int userId)
        {
            var theBids = await _bidsRepo.ViewAllBidsWithID(userId);

            if(!theBids.Any())
            {
                return OperationResult.Failure("Bids not found");
            }

            var bidsToReturn = theBids.Select(item =>
            new BidDTO()
            {
                BidId = item.BidId,
                BidAmount = item.BidAmount,
                BidTime = item.BidTime,
                UserId = item.UserId,
                AuctionId = item.AuctionId,

            }).ToList().AsReadOnly();


            return OperationResult.Success(bidsToReturn, "Bids found!");
        }

        public async Task<OperationResult> ViewBidsOnAuction(int auctionId)
        {
            var theBids = await _bidsRepo.ViewBidsOnAuction(auctionId);

            if(!theBids.Any())
            {
                return OperationResult.Failure("Bids not found");
            }

            var bidsToReturn = theBids.Select(item =>
            new BidDTO()
            {
                BidId = item.BidId,
                BidAmount = item.BidAmount,
                BidTime = item.BidTime,
                UserId = item.UserId,
                AuctionId = item.AuctionId,

            }).ToList().AsReadOnly();

            return OperationResult.Success(bidsToReturn,"Bids found!");
        }
    }
    
}
