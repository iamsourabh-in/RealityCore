using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AWS.Helper.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Cognito.Core
{
    public class CognitoHelper
    {

        private string POOL_ID = "us-east-1_AzJllAon5"; //ConfigurationManager.AppSettings["POOL_id"];
        private string CLIENTAPP_ID = "2u8gj25moem1u4l0s8q6m69gbc";// ConfigurationManager.AppSettings["CLIENT_id"];
        private string FED_POOL_ID = ""; // ConfigurationManager.AppSettings["FED_POOL_id"];
        private string CUSTOM_DOMAIN = ""; // ConfigurationManager.AppSettings["CUSTOMDOMAIN"];
        private string REGION = "us-east-1"; //Configuration.AppSettings["AWSREGION"];

        public CognitoHelper()
        {

        }

        public string GetCustomHostedURL()
        {
            return string.Format("https://{0}.auth.{1}.amazoncognito.com/login?response_type=code&client_id={2}&redirect_uri=https://sid343.reinvent-workshop.com/", CUSTOM_DOMAIN, REGION, CLIENTAPP_ID);
        }

        public async Task<bool> AdminSignUpUser(string username, string password, string email, string phonenumber, bool isAdmin = true)
        {
            AmazonCognitoIdentityProviderClient provider =
                   new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(REGION));

            //SignUpRequest signUpRequest = new SignUpRequest();
            //signUpRequest.Username = username;
            //signUpRequest.Password = password;


            //AttributeType attributeType = new AttributeType();
            //attributeType.Name = "phone_number";
            //attributeType.Value = phonenumber;
            //signUpRequest.UserAttributes.Add(attributeType);

            //AttributeType attributeType1 = new AttributeType();
            //attributeType1.Name = "email";
            //attributeType1.Value = email;
            //signUpRequest.UserAttributes.Add(attributeType1);

            AdminCreateUserRequest userRequest = new AdminCreateUserRequest();
            userRequest.Username = username;
            userRequest.UserPoolId = POOL_ID;
            userRequest.DesiredDeliveryMediums = new List<string>() { "EMAIL" };
            userRequest.TemporaryPassword = PasswordGenerator.GeneratePassword(true, true, true, true, false, 6);
            userRequest.UserAttributes.Add(new AttributeType { Name = "email", Value = email });
            userRequest.UserAttributes.Add(new AttributeType { Name = "phone_number", Value = phonenumber });

            try
            {
                //  SignUpResponse result = await provider.SignUpAsync(signUpRequest);
                AdminCreateUserResponse response = await provider.AdminCreateUserAsync(userRequest);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;

        }
        public async Task<bool> VerifyAccessCode(string username, string code)
        {
            AmazonCognitoIdentityProviderClient provider =
                   new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());
            ConfirmSignUpRequest confirmSignUpRequest = new ConfirmSignUpRequest();
            confirmSignUpRequest.Username = username;
            confirmSignUpRequest.ConfirmationCode = code;
            confirmSignUpRequest.ClientId = CLIENTAPP_ID;
            try
            {
                ConfirmSignUpResponse confirmSignUpResult = await provider.ConfirmSignUpAsync(confirmSignUpRequest);
                Console.WriteLine(confirmSignUpResult.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;

        }
        public async Task<CognitoUser> ResetPassword(string username)
        {
            AmazonCognitoIdentityProviderClient provider =
                   new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());

            CognitoUserPool userPool = new CognitoUserPool(this.POOL_ID, this.CLIENTAPP_ID, provider);

            CognitoUser user = new CognitoUser(username, this.CLIENTAPP_ID, userPool, provider);
            await user.ForgotPasswordAsync();
            return user;
        }
        public async Task<CognitoUser> UpdatePassword(string username, string code, string newpassword)
        {
            AmazonCognitoIdentityProviderClient provider =
                   new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());

            CognitoUserPool userPool = new CognitoUserPool(this.POOL_ID, this.CLIENTAPP_ID, provider);

            CognitoUser user = new CognitoUser(username, this.CLIENTAPP_ID, userPool, provider);
            await user.ConfirmForgotPasswordAsync(code, newpassword);
            return user;
        }
        public async Task<CognitoUser> ValidateUser(string username, string password)
        {
            AmazonCognitoIdentityProviderClient provider =
                    new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());

            CognitoUserPool userPool = new CognitoUserPool(this.POOL_ID, this.CLIENTAPP_ID, provider);

            CognitoUser user = new CognitoUser(username, this.CLIENTAPP_ID, userPool, provider);
            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
            {
                Password = password
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

        public async Task<bool> ResendTemporaryPasssword(string username)
        {
            AmazonCognitoIdentityProviderClient provider =
                    new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(REGION));

            AdminCreateUserRequest userRequest = new AdminCreateUserRequest();
            userRequest.Username = username;
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


        //Confirms user registration as an admin without using a confirmation code. Works on any user.

        public async Task<bool> AdminConfirmSignUp(string username)
        {
            AmazonCognitoIdentityProviderClient provider =
                    new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(REGION));

            AdminConfirmSignUpRequest userRequest = new AdminConfirmSignUpRequest();
            userRequest.Username = username;
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
    }
}
