using AuctionsApi_v2.Controllers;
using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.Domain.UtilityModels;
using AuctionsApi_v2.HelperMethods.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Test.Controllers
{
    public class AuctionsControllerTests
    {
        private readonly Mock<IAuctionsService> _mockService;
        private readonly Mock<ITokenReader> _mockTokenReader;
        private readonly AuctionsController _controller;

        public AuctionsControllerTests()
        {
            _mockService = new Mock<IAuctionsService>();
            _mockTokenReader = new Mock<ITokenReader>();
            _controller = new AuctionsController(_mockService.Object, _mockTokenReader.Object);
        }

        [Fact]
        public async Task CreateAuction_ShouldReturnOk_WhenAuctionIsCreated()
        {
            
            var request = new CreateAuction_Req { Title = "Test Auction" };
            var userId = 1;

            _mockTokenReader.Setup(t => t.ReadIdFromToken(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _mockService.Setup(s => s.CreateAuction(It.IsAny<CreateAuction_Req>()))
                .ReturnsAsync(OperationResult.Success("Created"));

        
            var result = await _controller.CreateAuction(request);

          
            var okResult = Assert.IsType<OkObjectResult>(result);
         
        }

        [Fact]
        public async Task CreateAuction_ShouldReturnUnauthorized_WhenUserIdInvalid()
        {
           
            _mockTokenReader.Setup(t => t.ReadIdFromToken(It.IsAny<ClaimsPrincipal>())).Returns(0);

          
            var result = await _controller.CreateAuction(new CreateAuction_Req());

           
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
          
        }

        [Fact]
        public async Task CreateAuction_ShouldReturnBadRequest_WhenServiceFails()
        {
          
            var request = new CreateAuction_Req { Title = "Failing Auction" };
            _mockTokenReader.Setup(t => t.ReadIdFromToken(It.IsAny<ClaimsPrincipal>())).Returns(1);
            _mockService.Setup(s => s.CreateAuction(It.IsAny<CreateAuction_Req>()))
                .ReturnsAsync(OperationResult.Failure("Creation failed"));

           
            var result = await _controller.CreateAuction(request);

           
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
          
           
        }
    }
}
