using AuctionsApi_v2.Core.Services;
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

namespace AuctionsApi_v2.Test.Services
{
    public class CategoriesServiceTest
    {
        private readonly Mock<ICategoriesRepo> _mockRepo;
        private readonly CategoriesService _service;

        public CategoriesServiceTest()
        {
            _mockRepo = new Mock<ICategoriesRepo>();
            _service = new CategoriesService(_mockRepo.Object);
        }

        [Fact]
        public async Task AddCategory_ShouldReturnSuccess_WhenRepoReturnsSuccess()
        {
            var req = new AddCategory_Req { Name = "Electronics", Description = "Tech stuff" };
            _mockRepo.Setup(r => r.AddCategory(It.IsAny<Categories>()))
                     .ReturnsAsync(OperationResult.Success(message: "Added"));

            var result = await _service.AddCategory(req);

            Assert.True(result.IsSuccces);
            _mockRepo.Verify(r => r.AddCategory(It.IsAny<Categories>()), Times.Once);
        }

       

        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnFailure_WhenNull()
        {
            _mockRepo.Setup(r => r.GetCategories()).ReturnsAsync((List<Categories>?)null);

            var result = await _service.GetCategoriesAsync();

            Assert.False(result.IsSuccces);
            Assert.Equal("Categories not found.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetOneCategory_ShouldReturnCategory_WhenExists()
        {
            var category = new Categories { Name = "Fashion", Description = "Clothing" };
            _mockRepo.Setup(r => r.GetOneCategory(1)).ReturnsAsync(category);

            var result = await _service.GetOneCategory(1);

            Assert.True(result.IsSuccces);
            var dto = result.Data as CategoryDTO;
            Assert.NotNull(dto);
            Assert.Equal("Fashion", dto.Name);
            
        }
    }
}
