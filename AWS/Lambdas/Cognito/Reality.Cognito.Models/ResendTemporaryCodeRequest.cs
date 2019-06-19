namespace Reality.Cognito.Models
{
    public class ResendTemporaryCodeRequest : CognitoRequestBase
    {
        public string Username { get; set; }
    }
}
