using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AuctionsApi_v2.Domain.Models.Auctions;

namespace AuctionsApi_v2.Domain.Models
{
    public class Bids
    {
        public int BidId { get; set; }

        [Column(TypeName = "decimal(19,3)")]
        public decimal BidAmount { get; set; }

        public DateTime BidTime { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int UserId { get; set; }
        public int AuctionId { get; set; }


        public Users User { get; set; }
        public Auctions Auction { get; set; }
    }
}
