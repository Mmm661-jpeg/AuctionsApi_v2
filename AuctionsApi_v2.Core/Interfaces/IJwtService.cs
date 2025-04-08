using AuctionsApi_v2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Users users);
    }
}
