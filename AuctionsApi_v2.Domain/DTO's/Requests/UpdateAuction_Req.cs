using AuctionsApi_v2.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.DTO_s.Requests
{
    public class UpdateAuction_Req
    {

        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public int AuctionId { get; set; }


        [StringLength(50, ErrorMessage = "Max lenght title is 50")]
        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Max lenght description is 250")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Starting price must be greater than 0")]
        public decimal? StartingPrice { get; set; }
        public DateTime? ClosingTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public int? CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
