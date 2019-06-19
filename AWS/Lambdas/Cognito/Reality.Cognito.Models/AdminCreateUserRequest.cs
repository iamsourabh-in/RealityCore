namespace Reality.Cognito.Models
{
    public class AdminCreateUserRequest : CognitoRequestBase
    {
        public string username { get; set; }
        public string email { get; set; }
        public string number { get; set; }


        public bool IsRequestValid()
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(number))
            {
                return true;
            }
            return false;
        }
    }
}
