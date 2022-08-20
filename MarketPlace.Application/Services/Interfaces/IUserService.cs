using MarketPlace.DataLayer.DTOs.Account;
using MarketPlace.DataLayer.Entities.Account;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        #region account

        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);
        Task<bool> IsUserExistByEmail(string email);
        Task<LoginUserResult> LoginUser(LoginUserDTO login);
        Task<User> GetUserByEmail(string email);
        Task<ForgotPasswordResult> RecoverPassword(ForgotPasswordDTO forgot);
        Task<bool> ActivateEmail(ActivateEmailDTO activate);
        Task<ChangePasswordResult> ChangePassword(ChangePasswordDTO change,long userId);
        Task<EditProfileDTO> GetUsreProfile(long userId);
        Task<EditProfileResult> EditUserProfile(EditProfileDTO profile,long userId,IFormFile avatarImage);

        #endregion
    }
}
