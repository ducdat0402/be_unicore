namespace UniCore.Helper.Constant
{
    public static class MessageConstants
    {
        public static class Auth
        {
            public const string LoginSuccess = "Auth.LoginSuccess";
            public const string RegisterSuccess = "Auth.RegisterSuccess";
            public const string LogoutSuccess = "Auth.LogoutSuccess";
            public const string RefreshTokenSuccess = "Auth.RefreshTokenSuccess";
            public const string SendOtpSuccess = "Auth.SendOtpSuccess";
            public const string VerifyOtpSuccess = "Auth.VerifyOtpSuccess";
            public const string ForgotPasswordSuccess = "Auth.ForgotPasswordSuccess";
            public const string ResetPasswordSuccess = "Auth.ResetPasswordSuccess";
            public const string GetMeSuccess = "Auth.GetMeSuccess";
            
            public const string InvalidCredentials = "Auth.InvalidCredentials";
            public const string InvalidRefreshToken = "Auth.InvalidRefreshToken";
            public const string EmailRegistered = "Auth.EmailRegistered";
            public const string UsernameTaken = "Auth.UsernameTaken";
            public const string DefaultRoleNotFound = "Auth.DefaultRoleNotFound";
            public const string InvalidOrExpiredOtp = "Auth.InvalidOrExpiredOtp";
            public const string UserNotFound = "Auth.UserNotFound";
            public const string IdentityNotFound = "Auth.IdentityNotFound";
        }

        public static class Student
        {
            public const string GetAllSuccess = "Student.GetAllSuccess";
            public const string GetByIdSuccess = "Student.GetByIdSuccess";
            public const string AddSuccess = "Student.AddSuccess";
            public const string UpdateSuccess = "Student.UpdateSuccess";
            public const string DeleteSuccess = "Student.DeleteSuccess";
        }

        public static class Role
        {
            public const string GetAllSuccess = "Role.GetAllSuccess";
            public const string GetByIdSuccess = "Role.GetByIdSuccess";
            public const string CreateSuccess = "Role.CreateSuccess";
            public const string UpdateSuccess = "Role.UpdateSuccess";
            public const string DeleteSuccess = "Role.DeleteSuccess";
            public const string GrantPermissionSuccess = "Role.GrantPermissionSuccess";
            public const string RevokePermissionSuccess = "Role.RevokePermissionSuccess";
            public const string GrantUserSuccess = "Role.GrantUserSuccess";
            public const string RevokeUserSuccess = "Role.RevokeUserSuccess";
            public const string NotFound = "Role.NotFound";
            public const string CodeAlreadyExists = "Role.CodeAlreadyExists";
        }

        public static class Permission
        {
            public const string GetAllSuccess = "Permission.GetAllSuccess";
            public const string GetByIdSuccess = "Permission.GetByIdSuccess";
            public const string CreateSuccess = "Permission.CreateSuccess";
            public const string UpdateSuccess = "Permission.UpdateSuccess";
            public const string DeleteSuccess = "Permission.DeleteSuccess";
            public const string GrantUserSuccess = "Permission.GrantUserSuccess";
            public const string RevokeUserSuccess = "Permission.RevokeUserSuccess";
            public const string NotFound = "Permission.NotFound";
            public const string CodeAlreadyExists = "Permission.CodeAlreadyExists";
        }

        public static class Announcement
        {
            public const string GetAllSuccess = "Announcement.GetAllSuccess";
            public const string GetByIdSuccess = "Announcement.GetByIdSuccess";
            public const string CreateSuccess = "Announcement.CreateSuccess";
            public const string UpdateSuccess = "Announcement.UpdateSuccess";
            public const string DeleteSuccess = "Announcement.DeleteSuccess";
            public const string PreviewSuccess = "Announcement.PreviewSuccess";
            public const string DeliveryReportSuccess = "Announcement.DeliveryReportSuccess";
            public const string GetPublicListSuccess = "Announcement.GetPublicListSuccess";
            public const string GetPublicByIdSuccess = "Announcement.GetPublicByIdSuccess";
            public const string NotFound = "Announcement.NotFound";
        }

        public static class Identity
        {
            public const string ScanCccdSuccess = "Identity.ScanCccdSuccess";
            public const string GetCccdSuccess = "Identity.GetCccdSuccess";
            public const string ImageRequired = "Identity.ImageRequired";
            public const string ImageQualityRejected = "Identity.ImageQualityRejected";
            public const string IdNumberAlreadyUsed = "Identity.IdNumberAlreadyUsed";
        }

        public static class FaceAuth
        {
            // Success messages
            public const string EnrollSuccess = "FaceAuth.EnrollSuccess";
            public const string SetPinSuccess = "FaceAuth.SetPinSuccess";
            public const string GetStatusSuccess = "FaceAuth.GetStatusSuccess";
            public const string LoginSuccess = "FaceAuth.LoginSuccess";
            public const string VerifyPinSuccess = "FaceAuth.VerifyPinSuccess";

            // Error messages
            public const string EnrollFailed = "FaceAuth.EnrollFailed";
            public const string SetPinFailed = "FaceAuth.SetPinFailed";
            public const string LoginFailed = "FaceAuth.LoginFailed";
            public const string VerifyPinFailed = "FaceAuth.VerifyPinFailed";

            // Validation messages
            public const string UserIdRequired = "FaceAuth.UserIdRequired";
            public const string ImagesRequired = "FaceAuth.ImagesRequired";
            public const string InvalidImageCount = "FaceAuth.InvalidImageCount";
            public const string InvalidImageType = "FaceAuth.InvalidImageType";
            public const string ImageTooLarge = "FaceAuth.ImageTooLarge";
            public const string PinRequired = "FaceAuth.PinRequired";
            public const string PinInvalidLength = "FaceAuth.PinInvalidLength";
            public const string PinMustBeDigits = "FaceAuth.PinMustBeDigits";
            public const string ConfirmPinRequired = "FaceAuth.ConfirmPinRequired";
            public const string PinMismatch = "FaceAuth.PinMismatch";
            public const string InvalidPin = "FaceAuth.InvalidPin";
            public const string PinLocked = "FaceAuth.PinLocked";

            // Status messages
            public const string NotEnrolled = "FaceAuth.NotEnrolled";
            public const string AlreadyEnrolled = "FaceAuth.AlreadyEnrolled";
            public const string AccountSuspended = "FaceAuth.AccountSuspended";
            public const string NoMatch = "FaceAuth.NoMatch";
            public const string ChallengeExpired = "FaceAuth.ChallengeExpired";
            public const string InvalidChallenge = "FaceAuth.InvalidChallenge";
        }

        public static class System
        {
            public const string ValidationFailed = "System.ValidationFailed";
            public const string UnexpectedError = "System.UnexpectedError";
            public const string ResourceNotFound = "System.ResourceNotFound";
            public const string UnauthorizedAccess = "System.UnauthorizedAccess";
        }
    }
}

