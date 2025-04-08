using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Data.Interfaces
{
    public interface IAuctionsRepo
    {
        Task<OperationResult> CreateAuction(Auctions auctions);

        Task<OperationResult> UpdateAuction(Auctions auctions);

        Task<OperationResult> DeleteAuctions(int auctionId, int userId);

        Task<Auctions> GetAuctionsWithId(int auctionId);

        Task<IEnumerable<Auctions>> GetAllAuctions();
    }
}
