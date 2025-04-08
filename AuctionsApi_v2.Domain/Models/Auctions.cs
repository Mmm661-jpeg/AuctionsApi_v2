using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.Models
{
    public class Auctions
    {
        public int AuctionId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(19,3)")]
        public decimal StartingPrice { get; set; }

        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }

        [Column(TypeName = "decimal(19,3)")]
        public decimal HighestBid { get; set; } = 0.00m;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsOpen =>
       DateTime.UtcNow >= OpeningTime.ToUniversalTime() &&
       DateTime.UtcNow < ClosingTime.ToUniversalTime();

        // Foreign keys
        public int CategoryId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public Categories Category { get; set; }
        public Users User { get; set; }
        public ICollection<Bids> Bids { get; set; } = new List<Bids>();
    }


}

