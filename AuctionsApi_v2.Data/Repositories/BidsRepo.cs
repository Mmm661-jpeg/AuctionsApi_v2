using AuctionsApi_v2.Data.DataModels;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Data.Repositories
{
    public class BidsRepo:IBidsRepo
    {
        private readonly AuctionDbContext _context;
        private readonly ILogger<BidsRepo> _logger;

        public BidsRepo(AuctionDbContext context, ILogger<BidsRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult> MakeBid(Bids bids)
        {

            try
            {
                var auctionToBidOn = await _context.Auctions.FirstOrDefaultAsync(a=>a.AuctionId == bids.AuctionId);

                var validationResult = ValidateMakeBid(auctionToBidOn, bids);

                if (!validationResult.IsSuccces)
                {
                    return validationResult;
                }


                await _context.Bids.AddAsync(bids);

                auctionToBidOn.HighestBid = bids.BidAmount;

                
                await _context.SaveChangesAsync();
                return OperationResult.Success(bids,"Succesful bid made");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error making bids failed");
                return OperationResult.Failure("Error make mid failed");
            }
        }

        public async Task<OperationResult> RemoveBid(int userId, int bidId) 
        {
            try
            {
                var bidToRemove = await _context.Bids
                    .FirstOrDefaultAsync(b => b.BidId.Equals(bidId) && b.UserId.Equals(userId));


                var auctionConnectedToBid = await _context.Auctions
                    .FirstOrDefaultAsync(a => a.AuctionId == bidToRemove.AuctionId);

                var validationResult = ValidateRemoveBid(auctionConnectedToBid, bidToRemove);

                if (!validationResult.IsSuccces)
                {
                    return validationResult;
                }



                _context.Bids.Remove(bidToRemove);
                await _context.SaveChangesAsync();

                return OperationResult.Success(bidToRemove.BidId,"Succesfully removed bid");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error Failed to remove {bidId} bid");
                return OperationResult.Failure("Error removing bid failed");
            }
        }

        public async Task<IEnumerable<Bids>> ViewAllBidsWithID(int userId)
        {
            try
            {
                var bidsFound = await _context.Bids.Where(b => b.UserId == userId).ToListAsync(); 

                if (bidsFound == null)
                {
                    _logger.LogDebug("Bid with userId: {userId} not found", userId);
                    return Enumerable.Empty<Bids>();
                }

                return bidsFound;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error get bid with user id failed");
                return Enumerable.Empty<Bids>();
            }
        }

        public async Task<IEnumerable<Bids>> ViewBidsOnAuction(int auctionId)
        {
            try
            {
                var bidsFound = await _context.Bids.Where(b => b.AuctionId == auctionId).ToListAsync();

                if (bidsFound == null)
                {
                    _logger.LogDebug("Bid with auctionId: {auctionId} not found", auctionId);
                    return Enumerable.Empty<Bids>();
                }

                return bidsFound;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get bid with auction id failed");
                return Enumerable.Empty<Bids>();
            }
        }


        private OperationResult ValidateMakeBid(Auctions auction, Bids bid)
        {

            if (bid == null)
            {
                _logger.LogWarning("MakeBid: Bid cannot be null");
                return OperationResult.Failure("Bid cannot be null");
            }

            if (auction == null)
            {
                _logger.LogWarning($"MakeBid: Auction with ID {bid.AuctionId} not found.");
                return OperationResult.Failure($"Auction with ID {bid.AuctionId} not found.");
            }

            if (auction.UserId == bid.UserId)
            {
                _logger.LogDebug($"MakeBid: User {bid.UserId} cannot bid on their own auction.");
                return OperationResult.Failure("You cannot bid on your own auction.");
            }

            if (!auction.IsOpen)
            {
                _logger.LogDebug($"MakeBid: Auction {auction.AuctionId} is closed.");
                return OperationResult.Failure("Auction is closed.");
            }

            if (auction.HighestBid >= bid.BidAmount)
            {
                _logger.LogDebug($"MakeBid: Bid {bid.BidAmount} is not higher than current highest bid {auction.HighestBid}.");
                return OperationResult.Failure("Bid amount must be higher than the current highest bid.");
            }

            return OperationResult.Success();
        }

        private OperationResult ValidateRemoveBid(Auctions auction, Bids bid)
        {

            if (bid == null)
            {
                _logger.LogWarning($"Bid to remove with bidId: {bid.BidId} and userId: {bid.UserId} not found");
                return OperationResult.Failure("Remove bid failed");
            }

            if (auction == null)
            {
                _logger.LogError("Something went wrong auction connected to bid to remove not found!!");
                return OperationResult.Failure("Error: Auction connected to bid not found..");
            }

            if (!auction.IsOpen)
            {
                _logger.LogDebug($"Cant remove bid if auction is closed");
                return OperationResult.Failure("Remove Bid failed, auction is closed");
            }

            if(auction.HighestBid == bid.BidAmount)
            {
                _logger.LogDebug($"Remove Bid failed, highest bid cantrmeove bid");
                return OperationResult.Failure("Cant remove bid if its the highest bid please contact customer support");
            }

            return OperationResult.Success();

        }
    }
}
