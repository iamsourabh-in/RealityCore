namespace Reality.Cognito.Models
{
    public class AdminCreateUserRequest : CognitoRequestBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        public bool IsRequestValid()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(PhoneNumber))
            {
                return true;
            }
            return false;
        }
    }
}
