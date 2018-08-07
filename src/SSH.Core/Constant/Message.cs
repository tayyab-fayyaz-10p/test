using SSH.Core.Resources;

namespace SSH.Core.Constant
{
    public static class Message
    {
        #region General
        public static readonly string NotFound = Messages.NotFound,
                                     InvalidObject = Messages.InvalidObject,
                                     AlreadyExist = Messages.AlreadyExist,
                                     ObjectRequired = Messages.ObjectRequired,
                                     SuccessResult = Messages.SuccessResult,
                                     Error = Messages.Error,
                                     NoContent = Messages.NoContent,
                                     RecordsUpdated = Messages.RecordsUpdated,
                                     RecordsDeleted = Messages.RecordsDeleted,
                                     DataInsertFailed = Messages.DataInsertFailed,
                                     DataUpdateFailed = Messages.DataUpdateFailed,
                                     DataDeleteFailed = Messages.DataDeleteFailed,
                                     RequiredData = Messages.RequiredData,
                                     DeviceInActive = Messages.DeviceInActive,
                                     InvalidReport = Messages.InvalidReport,
                                     InvalidPermission = Messages.InvalidPermission,
                                     UnChanged = Messages.UnChanged,
                                     UnAuthrized = Messages.UnAuthrized,
                                     InvalidLOVGroup = Messages.InvalidLOVGroup,
                                     UpdatedSuccess = Messages.UpdatedSuccess,
                                     InsertedSuccess = Messages.InsertedSuccess,
                                     DeletedSuccess = Messages.DeletedSuccess,
                                     AlreadyExistOrEmpty = Messages.AlreadyExistOrEmpty,
                                     PayrollCurrentDateError = Messages.PayrollCurrentDateError,
                                     BPMError = Messages.BPMError;

        #endregion

        #region DateTime

        public static readonly string DateInvalidDate = Messages.DateInvalidDate,
                                      DateInvalidDateTime = Messages.DateInvalidDateTime;

        #endregion

        #region User
        public static readonly string UserNewPasswordAndOldPasswordMatch = Messages.UserNewPasswordAndOldPasswordMatch,
                                     UserPasswordNotMatch = Messages.UserPasswordNotMatch,
                                     UserInvalidToken = Messages.UserInvalidToken,
                                     UserInvalidPassword = Messages.UserInvalidPassword,
                                     UserPasswordChangeSuccessfully = Messages.UserPasswordChangeSuccessfully,
                                     UserAssignPasswordSuccessfully = Messages.UserAssignPasswordSuccessfully,
                                     UserInvalidUser = Messages.UserInvalidUser,
                                     UserInvalidUserNameOrPassword = Messages.UserInvalidUserNameOrPassword,
                                     UserAccountNotExist = Messages.UserAccountNotExist,
                                     UserAccountNotExistCreateNew = Messages.UserAccountNotExistCreateNew,
                                     UserInvalidUserNameOrImeiNumber = Messages.UserInvalidUserNameOrImeiNumber,
                                     UserEmailNotConfirm = Messages.UserEmailNotConfirm,
                                     UserInvalidUserName = Messages.UserInvalidUserName,
                                     UserInvalidRole = Messages.UserInvalidRole,
                                     UserInvalidEmail = Messages.UserInvalidEmail,
                                     UserFirstAndLastName = Messages.UserFirstAndLastName,
                                     UserInactive = Messages.UserInactive,
                                     UserIncorrectOldPassword = Messages.UserIncorrectOldPassword,
                                     UserAlreadyExists = Messages.UserAlreadyExists,
                                     UserNameAlreadyExists = Messages.UserNameAlreadyExists,
                                     UserPasswordLength = Messages.UserPasswordLength,
                                     UserVerifiedUserName = Messages.UserVerifiedUserName,
                                     UserIvalidGrant = Messages.UserIvalidGrant,
                                     UserAccountLocked = Messages.UserAccountLocked,
                                     UserUnusualAttempts = Messages.UserUnusualAttempts,
                                     UserSignInAttemptsLeft = Messages.UserSignInAttemptsLeft,
                                     Successful = Messages.Successful,
                                     UserInActivation = Messages.UserInActivation,
                                     UserUnAuthorized = Messages.UserUnAuthorized,
                                     UserDeleted = Messages.UserDeleted,
                                     UserFreeze = Messages.UserFreeze,
                                     UserBlocked = Messages.UserBlocked,
                                     UserUnlockSuccessful = Messages.UserUnlockSuccessful;

        #endregion

        #region Address

        public static readonly string AddressNotFound = Messages.AddressNotFound;

        #endregion

        #region Application

        public static readonly string ApplicationApproved = Messages.ApplicationApproved,
                                      ApplicationReject = Messages.ApplicationReject,
                                      ApplicationRefer = Messages.ApplicationRefer,
                                      ApplicationExsits = Messages.ApplicationExsits;

        #endregion

        #region Mail

        public static readonly string SentMailNotFound = Messages.SentMailNotFound,
                                     OTPSubject = Mail.OTPSubject,
                                     OTPBody = Mail.OTPBody,
                                     CreateUserEmailSubject = Mail.CreateUserEmailSubject,
                                     CreateUserEmailBody = Mail.CreateUserEmailBody,
                                     LocationEmailSubject = Mail.LocationEmailSubject,
                                     LocationEmailBody = Mail.LocationEmailBody,
                                     UserRejectionEmailBody = Mail.UserRejectionEmailBody,
                                     UserRejectionEmailSubject = Mail.UserRejectionEmailSubject,
                                     JobCompletedEmailBody = Mail.JobCompletedEmailBody,
                                     JobCompletedEmailSubject = Mail.JobCompletedEmailSubject;

        #endregion

        #region User Hierarchy

        public static readonly string RegionalHierarchyFound = Messages.RegionalHierarchyFound,
                                     RegionalHierarchyInvalid = Messages.RegionalHierarchyInvalid,
                                     RegionalHierarchyUnauthorized = Messages.RegionalHierarchyUnauthorized;

        #endregion

        #region Attendance

        public static readonly string InvalidAttendance = Messages.InvalidAttendance;

        #endregion

        #region ChangeRequest

        public static readonly string ChangeRequestFailed = Messages.ChangeRequestFailed;

        #endregion

        #region OTP

        public static readonly string OneTimePinRequired = Messages.OneTimePinRequired,
                                     OneTimePinLength = Messages.OneTimePinLength,
                                     OneTimePinVerified = Messages.OneTimePinVerified,
                                     OneTimePinInvalid = Messages.OneTimePinInvalid,
                                     OneTimePinExpired = "OTP has been expired",
                                     OneTimePinSentToEmail = Messages.OneTimePinSentToEmail;

        #endregion

        #region LOV

        public static readonly string LOVExists = Messages.LOVExists;

        #endregion

        #region CSV And PDF Reports

        public static readonly string CsvReportCreated = Messages.CsvReportCreated,
                                    CsvReportCreationFailed = Messages.CsvReportCreationFailed,
                                    PdfReportCreated = Messages.PdfReportCreated,
                                    PdfReportCreationFailed = Messages.PdfReportCreationFailed;

        #endregion
    }
}
