namespace Reality.Cognito.Models
{
    public class AdminCreateUserRequest : CognitoRequestBase
    {
        public string username { get; set; }
        public string email { get; set; }
        public string number { get; set; }


        public bool IsRequestValid()
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(number))
            {
                return false;
            }
            return true;
        }
    }
}
