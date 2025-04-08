using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Data.Interfaces
{
    public interface IUsersRepo
    {
        Task<Users> GetUserWithUsername(string username);

        Task<OperationResult> Register(Users user);

        Task<OperationResult> Update(Users user);

        Task<OperationResult> Delete(int userId);

        Task<Users> GetUsersWithId(int userId);
    }
}
