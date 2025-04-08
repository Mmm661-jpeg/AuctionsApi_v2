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
    public class UsersRepo:IUsersRepo
    {
        private readonly AuctionDbContext _context;
        private readonly ILogger<UsersRepo> _logger;

        public UsersRepo(AuctionDbContext context, ILogger<UsersRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult> Delete(int userId)
        {
            try
            {
                var userToDelete = await _context.Users.FindAsync(userId);

                if (userToDelete == null)
                {
                    _logger.LogDebug("Delete User: Id: {userId} not found", userId);

                    return OperationResult.Failure("Deleting user failed");
                }

                _context.Users.Remove(userToDelete);

                await _context.SaveChangesAsync();

                return OperationResult.Success(message:"Deleting user was succesfull");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return OperationResult.Failure("Error deleting user");
            }
        }

        public async Task<Users> GetUsersWithId(int userId)
        {
            try
            {
                var result = await _context.Users.FindAsync(userId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get user with id: {userId} failed", userId);
                return null;
            }
        }

        public async Task<Users> GetUserWithUsername(string username)
        {
            try
            {
                var theUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

                if (theUser == null)
                {
                    _logger.LogDebug("Login: No match found for Username: {Username}", username);
                    return null;
                }

                return theUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for username: {Username}", username);
                return null;
            }

        }

        public async Task<OperationResult> Register(Users user)
        {
            if (user == null)
            {
                _logger.LogWarning("Register: User was null");
                return OperationResult.Failure("Register user failed");
            }

            try
            {
                var uniqueUsername = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == user.Username);

                if(uniqueUsername == null)
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    return OperationResult.Success(user.UserId,"Register successfull");
                }

                _logger.LogDebug("Register Username: {Username} was not unique", user.Username);
                return OperationResult.Failure("Register user failed");


            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong registering user");
                return OperationResult.Failure("Error registering user");
            }
        }

        public async Task<OperationResult> Update(Users user)
        {
            if(user == null)
            {
                _logger.LogWarning("Update failed: user is null");
                return OperationResult.Failure("Update user failed");
            }

            try
            {
                var existingUser = await _context.Users.FindAsync(user.UserId);
                if (existingUser == null)
                {
                    _logger.LogDebug("Update failed: user with ID {UserId} not found", user.UserId);
                    return OperationResult.Failure("Updating user failed");
                }

                _context.Entry(existingUser).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();

                return OperationResult.Success(message:"Updating user successfull");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating user.");
                return OperationResult.Failure("Error updating user");
            }
        }
    }
}
