using AuctionsApi_v2.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.DTO_s.Responses
{
    public class AuctionDTO
    {
        public int AuctionId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal StartingPrice { get; set; }

        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }

        public decimal HighestBid { get; set; }

        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
