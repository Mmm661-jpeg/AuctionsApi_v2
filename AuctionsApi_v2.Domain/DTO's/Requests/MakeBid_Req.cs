using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.DTO_s.Requests
{
    public class MakeBid_Req
    {

        //[Required]
        //[Range(1,int.MaxValue,ErrorMessage ="Id must be greater than 0")]
        public int UserId { get; set; }

        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Id must be greater than 0")]
        public int AuctionId { get; set; }

        [Required(ErrorMessage ="Bid amount is reqired")]
        [Range(0.01,double.MaxValue,ErrorMessage ="Bidamount must be greater than 0.01")]
        public decimal BidAmount { get; set; }
    }
}
