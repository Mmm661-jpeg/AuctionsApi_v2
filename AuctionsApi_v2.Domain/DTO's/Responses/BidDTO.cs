using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.DTO_s.Responses
{
    public class BidDTO
    {
        public int BidId { get; set; }

        public decimal BidAmount { get; set; }

        public DateTime BidTime { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public int AuctionId { get; set; }
    }
}
