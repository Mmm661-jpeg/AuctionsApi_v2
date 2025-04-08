using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.DTO_s.Requests
{
    public class AddCategory_Req
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        [StringLength(100)]
        public string Description { get; set; }
    }
}
