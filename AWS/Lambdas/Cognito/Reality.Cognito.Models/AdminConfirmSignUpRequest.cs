namespace Reality.Cognito.Models
{
    public class AdminConfirmSignUpRequest : CognitoRequestBase
    {
        public string Username { get; set; }
        public string TempCode { get; set; }

        public bool IsRequestValid()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(TempCode))
            {
                return true;
            }
            return false;
        }
    }
}
