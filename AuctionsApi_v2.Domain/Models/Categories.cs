using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AuctionsApi_v2.Domain.Models.Auctions;

namespace AuctionsApi_v2.Domain.Models
{
    public class Categories
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<Auctions> Auctions { get; set; } = new List<Auctions>();
    }
}
