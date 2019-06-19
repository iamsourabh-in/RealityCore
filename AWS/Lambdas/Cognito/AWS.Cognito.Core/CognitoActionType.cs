using System;
using System.Collections.Generic;
using System.Text;

namespace AWS.Cognito.Core
{
    public enum CognitoActionType
    {
        Login,
        AdminCreateUser,
        AdminConfirmUser,
        UserSignUp,
        UserConfirmSignUp,
        LoginWithTempPassword,
        ChangePassword,
        UpdatePassword,
        ForgotPassword,
        Logout,
        RefreshToken,
        UserEnabled,
        UserDisabled,
        AdminUpdateProfile,
        GetUserProfile,
        ResendMessage
    }
}
