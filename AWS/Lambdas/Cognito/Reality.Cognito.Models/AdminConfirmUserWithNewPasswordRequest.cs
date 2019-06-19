namespace Reality.Cognito.Models
{
    public class AdminConfirmUserWithNewPasswordRequest
    {
        public string Username { get; set; }
        public string TempPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
