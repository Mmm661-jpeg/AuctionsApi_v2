using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.Domain.DTO_s.Responses;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.Domain.UtilityModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Core.Services
{
    public class UsersService:IUsersService //Improvment: Allways return operantionsresult except for gets.
    {
        private readonly IUsersRepo _usersRepo;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IUsersRepo usersRepo, IJwtService jwtService,ILogger<UsersService> logger)
        {
            _usersRepo = usersRepo;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<OperationResult> Delete(int userId)
        {
            var result = await _usersRepo.Delete(userId);
            return result;
        }

        public async Task<OperationResult> GetUsersWithId(int userId)
        {
            var theUser = await _usersRepo.GetUsersWithId(userId);

            if(theUser == null)
            {
                return OperationResult.Failure("User not found");
            }

            var userToReturn = new UserDTO()
            {
                UserId = theUser.UserId,
                Username = theUser.Username,
                LastLogin = theUser.LastLogin,
            };

            return OperationResult.Success(userToReturn,"User found!");
        }

        public async Task<OperationResult> Login(Login_Req login_Req)
        {
            var theUser = await _usersRepo.GetUserWithUsername(login_Req.Username);

            if(theUser == null )
            {
                _logger.LogDebug($"User with username: {login_Req.Username} not found for login");
                return OperationResult.Failure("Username not found");
            }

            bool passVerification = VerifyPass(login_Req.Password, theUser.Password);

            if(passVerification)
            {
                var token = _jwtService.GenerateToken(theUser);

                return OperationResult.Success(token,"Succesfull login");
            }
            else
            {
                _logger.LogDebug("Login attempt failed password not verified");
                return OperationResult.Failure("Wrong password");
            }
        }

        public async Task<OperationResult> Register(Register_Req register_Req)
        {
            if(register_Req == null)
            {
                _logger.LogDebug("Register request cant be null!");
                return OperationResult.Failure("Register failed");
            }

            var hashedPassword = MakeHash(register_Req.Password);

            if(!string.IsNullOrWhiteSpace(hashedPassword))
            {
                var userToRegister = new Users()
                {
                    Username = register_Req.Username,
                    Password = hashedPassword,

                };

                var result = await _usersRepo.Register(userToRegister);

                return result;
            }
            else
            {
                return OperationResult.Failure("hashing password failed on register");
            }

           
        }

        public async Task<OperationResult> Update(UpdateUser_Req updateUser_Req)
        {
            var userToUpdate = await _usersRepo.GetUsersWithId(updateUser_Req.UserId);

            if(userToUpdate == null )
            {
                _logger.LogDebug("Update user no user found with input id");
                return OperationResult.Failure("Update failed");
            }

            userToUpdate.Username = updateUser_Req.Username ?? userToUpdate.Username;

            if(!string.IsNullOrWhiteSpace(updateUser_Req.Password))
            {
                var hashedUpdatePass = MakeHash(updateUser_Req.Password);
                if(string.IsNullOrWhiteSpace(hashedUpdatePass))
                {
                    return OperationResult.Failure("Hashing pass failed on update");
                }

                userToUpdate.Password = hashedUpdatePass;
            }

            var result = await _usersRepo.Update(userToUpdate);

            return result;
        }

        private bool VerifyPass(string password,string DBpass)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, DBpass);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong when doing verifypass");
                return false;
            }
        }

        private string MakeHash(string password)
        {
            try
            {
                var result = BCrypt.Net.BCrypt.HashPassword(password);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Something went wrong making hash");
                return null;
            }
        }
    }
}
