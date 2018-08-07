using SSH.Core.DTO;

namespace SSH.Test.Mock
{
    public static class ForgotPassword
    {
       public static string GetResetPasswordToken()
       {
           var resetPasswordToken = new ResetPasswordLinkDTO
           {
               UserName = "admin_admin"
           };
           return resetPasswordToken.UserName;
       }

       public static ForgotPasswordDTO ResetPassword(out string passwordToken)
       {          
           var forgotPassword = new ForgotPasswordDTO
           {
               NewPassword = "admin"
           };
           passwordToken = "f9c25cbd-e134-4711-b871-e09067aa5808";
           return forgotPassword;
       }
    }
}
