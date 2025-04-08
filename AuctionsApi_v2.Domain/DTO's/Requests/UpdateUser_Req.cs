using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.DTO_s.Requests
{
    public class UpdateUser_Req
    {
        //[Required(ErrorMessage = "User id is required")]
        //[Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public int UserId { get; set; }

        [MaxLength(50,ErrorMessage ="Max username lenght is 50")]
        [MinLength(4,ErrorMessage ="Minimum username lenght is 4")]
        public string? Username { get; set; }

        [MaxLength(255,ErrorMessage ="Max password lenght is 255")]
        [MinLength(3,ErrorMessage ="Minimum password lenght is 3")]
        public string? Password { get; set; }
    }
}
