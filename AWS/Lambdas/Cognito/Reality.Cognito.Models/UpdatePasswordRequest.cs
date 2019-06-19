namespace Reality.Cognito.Models
{
    public class UpdatePasswordRequest : CognitoRequestBase
    {
        public string Username { get; set; }
        public string TempPassword { get; set; }
        public string NewPassword { get; set; }

        public bool IsRequestValid()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(TempPassword) && !string.IsNullOrEmpty(NewPassword))
            {
                return true;
            }
            return false;
        }
    }
}
