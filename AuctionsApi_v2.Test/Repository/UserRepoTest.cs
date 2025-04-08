using AuctionsApi_v2.Data.DataModels;
using AuctionsApi_v2.Data.Repositories;
using AuctionsApi_v2.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AuctionsApi_v2.Domain.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Test.Repository
{
    public   class UserRepoTest
    {
        private UsersRepo CreateRepoWithInMemoryDb(out AuctionDbContext context)
        {
            var options = new DbContextOptionsBuilder<AuctionDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            context = new AuctionDbContext(options);

            var logger = Mock.Of<ILogger<UsersRepo>>();
            return new UsersRepo(context, logger);
        }

        [Fact]
        public async Task SuccesfullDelete()
        {
        
            var repo = CreateRepoWithInMemoryDb(out var context);
            var user = new Users { Username = "test", Password = "pass" };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

          
            var result = await repo.Delete(user.UserId);


            Assert.True(result.IsSuccces);
            Assert.Null(await context.Users.FindAsync(user.UserId));


        }

        [Fact]
        public async Task Register_When_Username_Exists()
        {
            var repo = CreateRepoWithInMemoryDb(out var context);
            var existingUser = new Users { Username = "duplicate", Password = "pass" };
            await context.Users.AddAsync(existingUser);
            await context.SaveChangesAsync();

            var user = new Users { Username = "duplicate", Password = "anotherpass" };
            var result = await repo.Register(user);

            Assert.False(result.IsSuccces);
        }

        [Fact]
        public async Task Update_User_Does_Not_Exist()
        {
            var repo = CreateRepoWithInMemoryDb(out var context);
            var user = new Users { UserId = 999, Username = "username", Password = "pass" };

            var result = await repo.Update(user);

            Assert.False(result.IsSuccces);
        }
    }
}
