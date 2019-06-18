namespace Reality.Cognito.Models
{
    public class ConfirmSignUpRequest : CognitoModel
    {
        public string username { get; set; }
        public string tempCode { get; set; }

    }
}
