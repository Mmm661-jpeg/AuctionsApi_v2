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
    public interface IAuctionsService
    {
        Task<OperationResult> CreateAuction(CreateAuction_Req auction_Req);

        Task<OperationResult> UpdateAuction(UpdateAuction_Req updateAuction_Req);

        Task<OperationResult> DeleteAuctions(int auctionId, int userId);

        Task<OperationResult> GetAuctionsWithId(int auctionId);

        Task<OperationResult> GetAllAuctions();
    }
}
