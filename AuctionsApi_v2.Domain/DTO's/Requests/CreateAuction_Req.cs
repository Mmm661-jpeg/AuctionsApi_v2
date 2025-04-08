using AuctionsApi_v2.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.DTO_s.Requests
{
    public class CreateAuction_Req
    {

        [Required(ErrorMessage ="Title is required")]
        [StringLength(50,ErrorMessage ="Max lenght title is 50")]
        public string Title { get; set; }

        [StringLength(250, ErrorMessage = "Max lenght description is 250")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Starting price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Starting price must be greater than 0")]
        public decimal StartingPrice { get; set; }

        [Required(ErrorMessage = "Opening time is required")]
        public DateTime OpeningTime { get; set; }

        [Required(ErrorMessage = "Closing time is required")]
        public DateTime ClosingTime { get; set; }


        // Foreign keys

        [Required(ErrorMessage = "Category id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public int CategoryId { get; set; }

        //[Required(ErrorMessage = "User id is required")]
        //[Range(1,int.MaxValue,ErrorMessage ="Id must be greater than 0")]
        public int UserId { get; set; }

    }
}
