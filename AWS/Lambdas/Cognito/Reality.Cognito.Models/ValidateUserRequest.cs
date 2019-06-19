namespace Reality.Cognito.Models
{
    public class ValidateUserRequest : CognitoRequestBase
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
