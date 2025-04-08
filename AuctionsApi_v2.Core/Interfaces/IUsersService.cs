using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.Domain.DTO_s.Responses;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Core.Interfaces
{
    public interface IUsersService
    {
        Task<OperationResult> Login(Login_Req login_Req); //rq post

        Task<OperationResult> Register(Register_Req register_Req); //rq post

        Task<OperationResult> Update(UpdateUser_Req updateUser_Req); //rq put

        Task<OperationResult> Delete(int userId); //delete

        Task<OperationResult> GetUsersWithId(int userId); //dto //get
    }
}
