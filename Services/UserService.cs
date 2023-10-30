using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebEvent.API.Model.DTO;
using WebEvent.API.Model.Entity;
using WebEvent.API.Model.Enums;
using WebEvent.API.Model.ErrorModel;
using WebEvent.API.Repository.Abstract;
using WebEvent.API.Services.Abstract;
using WebEvent.API.Services.Logger.Abstract;

namespace WebEvent.API.Services
{
    public class UserService:IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IAuthenticationService _authentication;
        public UserService(IMapper mapper, IRepositoryManager repository, ILoggerManager logger, IAuthenticationService authentication)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _authentication = authentication;
        }

        public async Task<bool> UpdateUser(UserDto dto)
        {
            //Get user from database
            User? user = await _repository.User.GetUserByEmail(dto.Email);

            if (user == null)
            {
                return false;
            }

            // Partial map UserDto to User entity
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;


            try
            {
                //Update entity
                await _repository.User.Update(user);

                //Try save changes to the database
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                
                throw new CustomException(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<bool> VerificationComplite(UserDto dto)
        {
            //Get user from database
            User? user = await _repository.User.GetUserByEmail(dto.Email);

            if (user == null)
            {
                return false;
            }

            user.IsVerified = true;

            try
            {
                //Update entity
                await _repository.User.Update(user);

                //Try save changes to the database
                await _repository.Save();
            }
            catch (DbUpdateException)
            {

                throw new CustomException(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<User> GetUser(string email)
        {
            //Get user with colaborators property
            User? user = await _repository.User.GetUserByEmail(email);

            if (user == null)
            {
                _logger.LogInfo($"No user found with email: {email}");
                throw new CustomException(message: "No user found") { StatusCode = StatusCode.DoesNotExist };
            }

            return user;
        }

        public async Task<User> GetUserByToken(string token)
        {
            //Get user with colaborators property
            User? user = await _repository.User.GetUSerByToken(token);

            if (user == null)
            {
                _logger.LogInfo($"No user found with token: {token}");
                throw new CustomException(message: "No user found") { StatusCode = StatusCode.DoesNotExist };
            }

            return user;
        }

        public async Task<bool> ChangePassword(ChangePasswordModel changePassword, string email)
        {
            //Get current user by email
            User? currentUser = await _repository.User.GetUserByEmail(email);

            bool isUserValid = await _authentication.ValidateUser(changePassword.CurrentPassword, email);

            //Validate old password
            if (!isUserValid)
            {
                return false;
            }

            //Create hash and salt of new password
            CreatePasswortHash(changePassword.NewPassword, out byte[] hash, out byte[] salt);

            //Set hash and salt to entity we got from the database
            //It's important before updating we must get entity from the database in current method
            currentUser.Password = hash;

            currentUser.PasswordSalt = salt;

            await _repository.User.Update(currentUser);

            //Try update changes
            try
            {
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                _logger.LogError("Can not update database. Save method in user service.");
                throw new CustomException(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<bool> Registrate(UserDto userDto)
        {
            //Create user in private method
            User user = MakeUser(userDto);

            //Check if email for user is alredy exist
            if (await _repository.User.GetUserByEmail(user.Email) != null)
            {
                return false;
            }

            await _repository.User.Create(user);

            //Check for updating entity
            try
            {
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                _logger.LogError("Can not update database. Save method in user service.");
                throw new CustomException(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        //Private method for creating new user
        private User MakeUser(UserDto dto)
        {
            User user = new User();

            //Create password hash
            CreatePasswortHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user = _mapper.Map<User>(dto);

            user.Password = passwordHash;

            user.PasswordSalt = passwordSalt;

            return user;

        }

        //Private method for creating password hash and salt
        private void CreatePasswortHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
