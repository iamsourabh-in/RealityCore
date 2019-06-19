using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Reality.Cognito.Models;

namespace AWS.Cognito.Core
{
    public interface ICognitoService
    {
        Task<bool> AdminConfirmSignUpAsync(Reality.Cognito.Models.AdminConfirmSignUpRequest request);
        Task<bool> AdminConfirmUserWithNewPassword(AdminConfirmUserWithTempPasswordRequest loginRequest);
        Task<Amazon.CognitoIdentityProvider.Model.AdminCreateUserResponse> AdminCreateUserAsync(Reality.Cognito.Models.AdminCreateUserRequest request);
        Task<bool> ConfirmSignUp(Reality.Cognito.Models.ConfirmSignUpRequest request);
        string GetCustomHostedURL();
        Task<bool> ResendTemporaryPasssword(ResendTemporaryCodeRequest request);
        Task<CognitoUser> ResetPassword(ResetPasswordRequest request);
        Task<CognitoUser> UpdatePassword(UpdatePasswordRequest request);
        Task<bool> UserSignUp(UserSignUpRequest signUpRequest);
        Task<CognitoUser> ValidateUser(ValidateUserRequest request);
    }
}