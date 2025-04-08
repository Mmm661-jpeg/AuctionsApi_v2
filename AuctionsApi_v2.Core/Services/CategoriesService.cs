using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.Domain.DTO_s.Responses;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuctionsApi_v2.Core.Services
{
    public class CategoriesService:ICategoriesService
    {
        private readonly ICategoriesRepo _categoriesRepo;

        public CategoriesService(ICategoriesRepo categoriesRepo)
        {
            _categoriesRepo = categoriesRepo;
        }

        public async Task<OperationResult> AddCategory(AddCategory_Req category_Req)
        {
            var newCategory = new Categories()
            {
                Name = category_Req.Name,
                Description = category_Req.Description,
            };

            var result = await _categoriesRepo.AddCategory(newCategory);
            return result;
        }

        public async Task<OperationResult> GetCategoriesAsync()
        {
            var theCategories = await _categoriesRepo.GetCategories();

            if(theCategories == null)
            {
                return OperationResult.Failure("Categories not found.");
            }

            var returnList = theCategories.Select(item =>
            new CategoryDTO()
            {
                Name = item.Name,
                Description = item.Description
            }).ToList().AsReadOnly();

            return OperationResult.Success(returnList,"Categories found!");
        }

        public async Task<OperationResult> GetOneCategory(int categoryId)
        {
            var theCategory = await _categoriesRepo.GetOneCategory(categoryId);

            if(theCategory == null)
            {
                return OperationResult.Failure("Category not found.");
            }

            var returnCategory = new CategoryDTO()
            {
                Name = theCategory.Name,
                Description = theCategory.Description
            };

            return OperationResult.Success(returnCategory,"Category found!");
        }

        public async Task<OperationResult> RemoveCategory(int categoryId)
        {
            var result = await _categoriesRepo.RemoveCategory(categoryId);
            return result;
        }
    }
}
