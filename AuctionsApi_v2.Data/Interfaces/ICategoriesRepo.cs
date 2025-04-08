using AuctionsApi_v2.Domain.DTO_s.Responses;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Data.Interfaces
{
    public interface ICategoriesRepo
    {
        Task<IEnumerable<Categories>> GetCategories();

        Task<OperationResult> AddCategory(Categories category);

       Task<OperationResult> RemoveCategory(int categoryId);

        Task<Categories> GetOneCategory(int categoryId);
    }
}
