using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static AuctionsApi_v2.Domain.Models.Auctions;

namespace AuctionsApi_v2.Domain.Models
{
    public class Users
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }


        public ICollection<Auctions> Auctions { get; set; } = new List<Auctions>();
        public ICollection<Bids> Bids { get; set; } = new List<Bids>();
    }
}
