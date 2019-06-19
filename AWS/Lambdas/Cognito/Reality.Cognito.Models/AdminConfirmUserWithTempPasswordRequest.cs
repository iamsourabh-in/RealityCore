using System;

namespace Reality.Cognito.Models
{
    public class AdminConfirmUserWithTempPasswordRequest : CognitoRequestBase
    {
        public string Username { get; set; }
        public string TempPassword { get; set; }
        public string NewPassword { get; set; }

        public bool IsRequestValid()
        {
            throw new NotImplementedException();
        }
    }
}
