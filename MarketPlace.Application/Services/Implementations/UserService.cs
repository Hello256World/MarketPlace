using MarketPlace.Application.Extensions;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.DataLayer.DTOs.Account;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region constructor

        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IEmailSender _emailSender;

        public UserService(IGenericRepository<User> userRepository, IPasswordHelper passwordHelper, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _emailSender = emailSender;
        }

        #endregion

        #region account

        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO register)
        {
            if (!await IsUserExistByEmail(register.Email))
            {
                var user = new User
                {
                    Email = register.Email,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Password = _passwordHelper.EncodePasswordMd5(register.ConfirmPassword),
                    EmailActiveCode = new Random().Next(100000, 999999).ToString(),
                    MobileActiveCode = Guid.NewGuid().ToString(),
                };

                await _userRepository.AddEntity(user);
                await _userRepository.SaveChanges();
                await _emailSender.SendMail(user.Email, user.EmailActiveCode, "فعالسازی ایمیل", "کد فعالسازی ایمیل");

                return RegisterUserResult.Success;
            }

            return RegisterUserResult.UserExist;
        }

        public async Task<bool> IsUserExistByEmail(string email)
        {
            return await _userRepository.GetQuery().AsQueryable().AnyAsync(x => x.Email == email);
        }

        public async Task<LoginUserResult> LoginUser(LoginUserDTO login)
        {
            var user = await _userRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Email == login.Email);
            if (user == null) return LoginUserResult.NotFound;
            if (!user.IsEmailActive) return LoginUserResult.NotActive;
            if (_passwordHelper.EncodePasswordMd5(login.Password) != user.Password) return LoginUserResult.NotFound;
            return LoginUserResult.Success;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<ForgotPasswordResult> RecoverPassword(ForgotPasswordDTO forgot)
        {
            var user = await _userRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Email == forgot.Email);

            if (user == null) return ForgotPasswordResult.NotFound;

            var newPassword = new Random().Next(100000, 999999).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(newPassword);
            _userRepository.EditeEntity(user);
            await _userRepository.SaveChanges();
            await _emailSender.SendMail(user.Email, newPassword, "بازیابی رمز عبور", "رمز جدید شما");
            return ForgotPasswordResult.Success;
        }

        public async Task<bool> ActivateEmail(ActivateEmailDTO activate)
        {
            var user = await _userRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Email == activate.Email);
            if (user != null)
            {
                if (user.EmailActiveCode == activate.ActivateCode)
                {
                    user.IsEmailActive = true;
                    user.EmailActiveCode = new Random().Next(100000, 999999).ToString();
                    _userRepository.EditeEntity(user);
                    await _userRepository.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        public async Task<ChangePasswordResult> ChangePassword(ChangePasswordDTO change, long userId)
        {
            var user = await _userRepository.GetEntityById(userId);
            if (user.Password != _passwordHelper.EncodePasswordMd5(change.CurrentPassword)) return ChangePasswordResult.WrongCurrentPass;
            if (user.Password == _passwordHelper.EncodePasswordMd5(change.ConfirmNewPassword)) return ChangePasswordResult.DuplicatPassword;

            var newPassword = _passwordHelper.EncodePasswordMd5(change.NewPassword);
            user.Password = newPassword;
            _userRepository.EditeEntity(user);
            await _userRepository.SaveChanges();

            return ChangePasswordResult.Success;
        }

       public async Task<EditProfileDTO> GetUsreProfile(long userId)
        {
            var user = await _userRepository.GetEntityById(userId);
            if (user == null) return null;

            return new EditProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar
            };
        }

       public async Task<EditProfileResult> EditUserProfile(EditProfileDTO profile, long userId, IFormFile avatarImage)
        {
            var user = await _userRepository.GetEntityById(userId);

            if(user == null) return EditProfileResult.NotFound;

            user.FirstName = profile.FirstName.Sanitize();
            user.LastName = profile.LastName.Sanitize();

            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatarImage.FileName);
                avatarImage.AddImageToServer(imageName,PathExtensions.UserAvatarOriginServer,100,100,PathExtensions.UserAvatarThumbServer,user.Avatar);
                user.Avatar = imageName;
            }

            _userRepository.EditeEntity(user);
            await _userRepository.SaveChanges();

            return EditProfileResult.Success;
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _userRepository.DisposeAsync();
        }

        #endregion
    }
}
