using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.Domain.DTO_s.Responses;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Core.Interfaces
{
    public interface IBidsService
    {
        Task<OperationResult> MakeBid(MakeBid_Req makeBid_Req);

        Task<OperationResult> RemoveBid(int userId, int bidId);

        Task<OperationResult> ViewAllBidsWithID(int userId);

        Task<OperationResult> ViewBidsOnAuction(int auctionId);
    }
}
