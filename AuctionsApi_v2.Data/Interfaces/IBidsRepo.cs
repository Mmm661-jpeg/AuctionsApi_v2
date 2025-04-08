using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Data.Interfaces
{
    public interface IBidsRepo
    {
        Task<OperationResult> MakeBid(Bids bids);

        Task<OperationResult> RemoveBid(int userId, int bidId);

        Task<IEnumerable<Bids>> ViewAllBidsWithID(int userId);

        Task<IEnumerable<Bids>> ViewBidsOnAuction(int auctionId);
    }
}
