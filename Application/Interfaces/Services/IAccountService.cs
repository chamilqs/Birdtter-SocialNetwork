using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Core.Application.ViewModels.User;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<SaveIdentityUserViewModel> UpdateUserAsync(SaveIdentityUserViewModel vm);
        Task SignOutAsync();
    }
}