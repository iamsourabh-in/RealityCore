using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AWS.Helper.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AWSModels = Amazon.CognitoIdentityProvider.Model;
using Real = Reality.Cognito.Models;

namespace AWS.Cognito.Core
{
    public class CognitoService : ICognitoService
    {

        private string POOL_ID = "us-east-1_AzJllAon5"; //ConfigurationManager.AppSettings["POOL_id"];
        private string CLIENTAPP_ID = "2u8gj25moem1u4l0s8q6m69gbc";// ConfigurationManager.AppSettings["CLIENT_id"];
        private string FED_POOL_ID = ""; // ConfigurationManager.AppSettings["FED_POOL_id"];
        private string CUSTOM_DOMAIN = ""; // ConfigurationManager.AppSettings["CUSTOMDOMAIN"];
        private string REGION = "us-east-1"; //Configuration.AppSettings["AWSREGION"];

        public CognitoService()
        {

        }

        public string GetCustomHostedURL()
        {
            return string.Format("https://{0}.auth.{1}.amazoncognito.com/login?response_type=code&client_id={2}&redirect_uri=https://sid343.reinvent-workshop.com/", CUSTOM_DOMAIN, REGION, CLIENTAPP_ID);
        }

        #region Admin Actions (Create and Confirm User)

        /// <summary>
        /// Used to Create User in cognito. other attributes can be provided but are not mandatory.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<AdminCreateUserResponse> AdminCreateUserAsync(Real.AdminCreateUserRequest request)
        {
            AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(REGION));

            AWSModels.AdminCreateUserRequest userRequest = new AWSModels.AdminCreateUserRequest();
            userRequest.Username = request.Username;
            userRequest.UserPoolId = POOL_ID;
            userRequest.DesiredDeliveryMediums = new List<string>() { "EMAIL" };
            userRequest.TemporaryPassword = PasswordGenerator.GeneratePassword(true, true, true, true, false, 6);
            userRequest.UserAttributes.Add(new AttributeType { Name = "email", Value = request.Email });
            userRequest.UserAttributes.Add(new AttributeType { Name = "phone_number", Value = request.PhoneNumber });

            AdminCreateUserResponse response = null;

            try
            {
                response = await provider.AdminCreateUserAsync(userRequest);
            }
            catch (CodeDeliveryFailureException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.CodeDeliveryFailureException, ex.StackTrace, ex.Message);
            }
            catch (InternalErrorException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.InternalErrorException, ex.StackTrace, ex.Message);
            }
            catch (InvalidLambdaResponseException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.InvalidLambdaResponseException, ex.StackTrace, ex.Message);
            }
            catch (InvalidParameterException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.InvalidParameterException, ex.StackTrace, ex.Message);
            }
            catch (InvalidUserPoolConfigurationException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.InvalidUserPoolConfigurationException, ex.StackTrace, ex.Message);
            }
            catch (PasswordResetRequiredException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.PasswordResetRequiredException, ex.StackTrace, ex.Message);
            }
            catch (ResourceNotFoundException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.ResourceNotFoundException, ex.StackTrace, ex.Message);
            }
            catch (TooManyRequestsException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.TooManyRequestsException, ex.StackTrace, ex.Message);
            }
            catch (UnexpectedLambdaException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.UnexpectedLambdaException, ex.StackTrace, ex.Message);
            }
            catch (UserLambdaValidationException ex)
            {
                if (ex.Message == "User account already exists")
                {
                    ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.UserNameExistsException, ex.StackTrace, ex.Message);

                }
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.UserLambdaValidationException, ex.StackTrace, ex.Message);
            }
            catch (UserNotConfirmedException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.UserNotConfirmedException, ex.StackTrace, ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.UserNotFoundException, ex.StackTrace, ex.Message);
            }
            catch (AmazonCognitoIdentityProviderException ex)
            {
                if (ex.Message == "User account already exists")
                    ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.UserNameExistsException, ex.StackTrace, ex.Message);

                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.AmazonCognitoIdentityProviderException, ex.StackTrace, ex.Message);
            }
            catch (Exception ex)
            {
                ThrowCustomException(CognitoActionType.AdminCreateUser, ExceptionConstants.ErrorException, ex.StackTrace, ex.Message);
            }
            return response;

        }

        /// <summary>
        /// Confirms user registration as an admin without using a confirmation code. Works on any user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> AdminConfirmSignUpAsync(Real.AdminConfirmSignUpRequest request)
        {
            AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(REGION));

            AdminConfirmSignUpRequest userRequest = new AdminConfirmSignUpRequest();
            userRequest.Username = request.Username;
            userRequest.UserPoolId = POOL_ID;

            try
            {
                AdminConfirmSignUpResponse response = await provider.AdminConfirmSignUpAsync(userRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;

        }

        /// <summary>
        /// Confirms user registration as an admin with using a confirmation code.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<bool> AdminConfirmUserWithNewPassword(Real.AdminConfirmUserWithTempPasswordRequest loginRequest)
        {
            var client = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(REGION));
            var dictTypeAuthParam = new Dictionary<string, string> { { "USERNAME", loginRequest.Username }, { "PASSWORD", loginRequest.TempPassword } };

            AdminInitiateAuthRequest req = new AdminInitiateAuthRequest()
            {
                AuthFlow = new AuthFlowType(AuthFlowType.ADMIN_NO_SRP_AUTH),
                ClientId = CLIENTAPP_ID,
                UserPoolId = POOL_ID,
                AuthParameters = dictTypeAuthParam
            };

            var response = await client.AdminInitiateAuthAsync(req);
            var dictTypeChallangeResponse = new Dictionary<string, string>
            {
                {"USERNAME",  loginRequest .Username},
                {"NEW_PASSWORD", loginRequest.NewPassword}
            };

            var respondRequest = new AdminRespondToAuthChallengeRequest()
            {
                ChallengeName = new ChallengeNameType(ChallengeNameType.NEW_PASSWORD_REQUIRED),
                ClientId = CLIENTAPP_ID,
                ChallengeResponses = dictTypeChallangeResponse,
                Session = response.Session,
                UserPoolId = POOL_ID

            };
            var respondResponse = await client.AdminRespondToAuthChallengeAsync(respondRequest);
            return true;

        }


        /// <summary>
        /// This send the temporary code again to the admin created user which is now not activate
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> ResendTemporaryPasssword(Real.ResendTemporaryCodeRequest request)
        {
            AmazonCognitoIdentityProviderClient provider =
                    new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(REGION));

            AdminCreateUserRequest userRequest = new AdminCreateUserRequest();
            userRequest.Username = request.Username;
            userRequest.UserPoolId = POOL_ID;
            userRequest.DesiredDeliveryMediums = new List<string>() { "EMAIL" };
            userRequest.MessageAction = MessageActionType.RESEND;
            userRequest.TemporaryPassword = PasswordGenerator.GeneratePassword(true, true, true, true, false, 6);

            try
            {
                AdminCreateUserResponse response = await provider.AdminCreateUserAsync(userRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;

        }


        #endregion

        #region User Actions (SignUp,Login,Reset/Update Password)

        /// <summary>
        /// Use to Register User
        /// </summary>
        /// <param name="signUpRequest"></param>
        /// <returns></returns>
        public async Task<bool> UserSignUp(Real.UserSignUpRequest signUpRequest)
        {
            AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());

            SignUpRequest request = new SignUpRequest();
            request.Username = signUpRequest.username;
            request.Password = signUpRequest.password;
            request.UserAttributes.Add(new AttributeType { Name = "email", Value = signUpRequest.email });
            request.UserAttributes.Add(new AttributeType { Name = "phone_number", Value = signUpRequest.number });
            await provider.SignUpAsync(request);

            return true;
        }

        /// <summary>
        /// Verify the access code to confirm signup when force password change is not imposed.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmSignUp(Real.ConfirmSignUpRequest request)
        {
            AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());
            ConfirmSignUpRequest confirmSignUpRequest = new ConfirmSignUpRequest();
            confirmSignUpRequest.Username = request.Username;
            confirmSignUpRequest.ConfirmationCode = request.TempCode;
            confirmSignUpRequest.ClientId = CLIENTAPP_ID;
            try
            {
                ConfirmSignUpResponse confirmSignUpResult = await provider.ConfirmSignUpAsync(confirmSignUpRequest);
                Console.WriteLine(confirmSignUpResult.ToString());
            }
            catch (Exception ex)
            {
                ThrowCustomException(CognitoActionType.UserConfirmSignUp, ExceptionConstants.InternalServerErrorException, ex.StackTrace, ex.Message);
            }

            return true;

        }

        /// <summary>
        ///  Allow user to reset his/her password by sending a reset code
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<CognitoUser> ResetPassword(Real.ResetPasswordRequest request)
        {
            AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());

            CognitoUserPool userPool = new CognitoUserPool(this.POOL_ID, this.CLIENTAPP_ID, provider);

            CognitoUser user = new CognitoUser(request.Username, this.CLIENTAPP_ID, userPool, provider);
            await user.ForgotPasswordAsync();
            return user;
        }

        /// <summary>
        /// Update Password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="code"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        public async Task<CognitoUser> UpdatePassword(Real.UpdatePasswordRequest request)
        {
            AmazonCognitoIdentityProviderClient provider =
                   new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());

            CognitoUserPool userPool = new CognitoUserPool(this.POOL_ID, this.CLIENTAPP_ID, provider);

            CognitoUser user = new CognitoUser(request.Username, this.CLIENTAPP_ID, userPool, provider);
            await user.ConfirmForgotPasswordAsync(request.TempPassword, request.NewPassword);
            return user;
        }

        /// <summary>
        /// Validate user in Cognito
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<CognitoUser> ValidateUser(Real.ValidateUserRequest request)
        {
            AmazonCognitoIdentityProviderClient provider =
                    new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());

            CognitoUserPool userPool = new CognitoUserPool(this.POOL_ID, this.CLIENTAPP_ID, provider);

            CognitoUser user = new CognitoUser(request.Username, this.CLIENTAPP_ID, userPool, provider);
            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
            {
                Password = request.Password
            };


            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
            if (authResponse.AuthenticationResult != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }


        #endregion

        #region Class Helpers
        public async Task<string> GetS3BucketsAsync(CognitoUser user)
        {
            CognitoAWSCredentials credentials =
               user.GetCognitoAWSCredentials(FED_POOL_ID, new AppConfigAWSRegion().Region);
            StringBuilder bucketlist = new StringBuilder();

            bucketlist.Append("================Cognito Credentails==================\n");
            bucketlist.Append("Access Key - " + credentials.GetCredentials().AccessKey);
            bucketlist.Append("\nSecret - " + credentials.GetCredentials().SecretKey);
            bucketlist.Append("\nSession Token - " + credentials.GetCredentials().Token);

            bucketlist.Append("\n================User Buckets==================\n");

            using (var client = new AmazonS3Client(credentials))
            {
                ListBucketsResponse response =
                    await client.ListBucketsAsync(new ListBucketsRequest()).ConfigureAwait(false);

                foreach (S3Bucket bucket in response.Buckets)
                {
                    bucketlist.Append(bucket.BucketName);

                    bucketlist.Append("\n");
                }
            }
            Console.WriteLine(bucketlist.ToString());
            return bucketlist.ToString();
        }


        /// <summary>
        /// Log and throw a custom Exception
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="customException"></param>
        /// <param name="stackTrace"></param>
        /// <param name="message"></param>
        /// <param name="username"></param>
        /// <param name="sourceIp"></param>
        public void ThrowCustomException(CognitoActionType actionType, string customException, string stackTrace, string message, string username = "", string sourceIp = "")
        {
            var UnixCurrentTimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            LambdaLogger.Log($" Time: { UnixCurrentTimeStamp } | Action Type: {actionType.ToString()}, Exception: {customException}, Message: {message}, User: {username}, IP: {sourceIp} ");
            LambdaLogger.Log($" Time: { UnixCurrentTimeStamp } | StackTrace: {stackTrace}");
            throw new Exception(customException);
        }
        #endregion
    }
}
