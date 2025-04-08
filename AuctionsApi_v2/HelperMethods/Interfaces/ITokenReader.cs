using System.Security.Claims;

namespace AuctionsApi_v2.HelperMethods.Interfaces
{
    public interface ITokenReader
    {
        public int ReadIdFromToken(ClaimsPrincipal user);
    }
}
