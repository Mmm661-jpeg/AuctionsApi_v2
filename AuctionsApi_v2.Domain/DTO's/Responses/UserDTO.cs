using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.DTO_s.Responses
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}
