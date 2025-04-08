using AuctionsApi_v2.Data.DataModels;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Data.Repositories
{
    public class AuctionsRepo:IAuctionsRepo
    {
        private readonly AuctionDbContext _context;
        private readonly ILogger<AuctionsRepo> _logger;

        public AuctionsRepo(AuctionDbContext context, ILogger<AuctionsRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult> CreateAuction(Auctions auctions)
        {
            if(auctions == null)
            {
                _logger.LogWarning("Create Auction: Auction can not be null!");
                return OperationResult.Failure("Create auction failed");
            }

            try
            {
                await _context.Auctions.AddAsync(auctions);

                await _context.SaveChangesAsync();

                return OperationResult.Success(auctions.AuctionId,"Succesfully made auction");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error failed to create auction");
                return OperationResult.Failure("Error create auction failed");
            }
        }

        public async Task<OperationResult> DeleteAuctions(int auctionId,int userId)
        {
            try
            {
                var auctionToDelete = await _context.Auctions
               .Include(a => a.Bids)  
               .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

                if (auctionToDelete == null)
                {
                    _logger.LogWarning("Auction with Id: {auctionId} not found", auctionId);
                    return OperationResult.Failure("Deleting auction failed");
                }
                else if(auctionToDelete.UserId != userId)
                {
                    _logger.LogDebug("Delete Auction failed only owner can delete");
                    return OperationResult.Failure("Deleting auction failed only owner can delete");
                }

                if (auctionToDelete.Bids.Any()) //no cascade so manual delete..
                {
                    _context.Bids.RemoveRange(auctionToDelete.Bids);  
                }

                _context.Auctions.Remove(auctionToDelete);

                await _context.SaveChangesAsync();

                return OperationResult.Success(message:"Succesfully deleted auction");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error failed to delete auction");
                return OperationResult.Failure("Error deleting auction");
            }
        }

        public async Task<IEnumerable<Auctions>> GetAllAuctions()
        {
            try
            {
                return await _context.Auctions.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error failed to get all auctions");
                return Enumerable.Empty<Auctions>();
            }
        }

        public async Task<Auctions> GetAuctionsWithId(int auctionId)
        {
            try
            {
                var auctionToGet = await _context.Auctions.FindAsync(auctionId);

                if(auctionToGet == null)
                {
                    _logger.LogDebug("Auction with Id: {auctionId} not found", auctionId);
                    return null;
                }

                return auctionToGet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error failed to get auction with id: {auctionId}",auctionId);
                return null;
            }
        }

        public async Task<OperationResult> UpdateAuction(Auctions auctions)
        {
            try
            {
                var existingAuction = await _context.Auctions.FindAsync(auctions.AuctionId);

                if(existingAuction == null)
                {
                    _logger.LogWarning("Update auction failed Auction with Id: {auctionId} not found", auctions.AuctionId);
                    return OperationResult.Failure("Updating auction failed");
                }
                else if(existingAuction.UserId != auctions.UserId)
                {
                    _logger.LogDebug($"Update auction: userId of auction to update does not match db userId for auction. userId req:{auctions.UserId} userId db: {existingAuction.UserId}");
                    return OperationResult.Failure("Update auction failed");
                }

                _context.Entry(existingAuction).CurrentValues.SetValues(auctions);

                await _context.SaveChangesAsync();

                return OperationResult.Success(existingAuction.UserId,"Succesfully updated auction");
            }
            catch(Exception ex)
            {
                _logger.LogError("Error failed to update auction");
                return OperationResult.Failure("Error updating auction");
            }
        }
    }
}
