namespace Reality.Cognito.Models
{
    public class ResendTemporaryCodeRequest : CognitoRequestBase
    {
        public string username { get; set; }
    }
}
