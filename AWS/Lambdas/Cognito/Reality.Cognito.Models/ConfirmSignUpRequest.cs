namespace Reality.Cognito.Models
{
    public class ConfirmSignUpRequest : CognitoRequestBase
    {
        public string username { get; set; }
        public string tempCode { get; set; }

    }
}
