namespace Reality.Cognito.Models
{
    public class AdminConfirmSignUpRequest : CognitoRequestBase
    {
        public string username { get; set; }
        public string tempCode { get; set; }

        public bool IsRequestValid()
        {
            return true;
        }
    }
}
