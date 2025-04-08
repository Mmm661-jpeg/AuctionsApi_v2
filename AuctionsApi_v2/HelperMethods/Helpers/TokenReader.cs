using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.HelperMethods.Interfaces;
using System.Security.Claims;

namespace AuctionsApi_v2.HelperMethods.Helpers
{
    public class TokenReader:ITokenReader
    {
        public int ReadIdFromToken(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst("UserID");

            if (idClaim != null && int.TryParse(idClaim.Value, out int result))
            {
                return result;
            }
            else
            {
                return -1;
            }


        }
    }
}
