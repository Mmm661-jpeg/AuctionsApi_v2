using AuctionsApi_v2.Data.DataModels;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Data.Repositories
{
    public class CategoriesRepo:ICategoriesRepo
    {
        private readonly AuctionDbContext _context;
        private readonly ILogger<CategoriesRepo> _logger;

        public CategoriesRepo(AuctionDbContext context,ILogger<CategoriesRepo> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<OperationResult> AddCategory(Categories category)
        {

            if (category == null)
            {
                _logger.LogWarning("Add Category: No nulls!");
                return OperationResult.Failure("Adding category failed");
            }

            try
            {

                await _context.Categories.AddAsync(category);

                await _context.SaveChangesAsync();

                return OperationResult.Success(category.CategoryId, "Succesfully added category");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                return OperationResult.Failure("Error adding category failed");
            }

        }

        public async Task<IEnumerable<Categories>> GetCategories()
        {
           try
            {
                return await _context.Categories.ToListAsync();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                return Enumerable.Empty<Categories>();
            }
        }

        public async Task<Categories> GetOneCategory(int categoryId)
        {
            try
            {
                var result = await _context.Categories.FindAsync(categoryId);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category with ID {CategoryId}", categoryId);
                return null;
            }
        }

        public async Task<OperationResult> RemoveCategory(int categoryId)
        {
            try
            {
                var categoryToRemove = await _context.Categories.FindAsync(categoryId);
                   

                if (categoryToRemove == null)
                {
                    _logger.LogDebug("Remove Category: No match found for ID {CategoryId}", categoryId);
                    return OperationResult.Failure("Removing category failed");
                }

                _context.Categories.Remove(categoryToRemove);

                await _context.SaveChangesAsync();

                return OperationResult.Success(categoryToRemove.CategoryId,"Remving category succesfull");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error removing category");
                return OperationResult.Failure("Error removing category");
            }
        }
    }
}
