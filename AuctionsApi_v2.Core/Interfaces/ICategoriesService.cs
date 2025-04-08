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
    public interface ICategoriesService
    {
        Task<OperationResult> GetCategoriesAsync(); 

        Task<OperationResult> AddCategory(AddCategory_Req category_Req); 

        Task<OperationResult> RemoveCategory(int categoryId); 

        Task<OperationResult> GetOneCategory(int categoryId); 
    }
}
