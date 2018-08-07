namespace SSH.Core.Constant
{
    public static class Validations
    {
        public const string EmailAddress = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+";
        public const string Alphabet = @"^[a-zA-Z ]*$";
        public const string DateTimeFormate = @"yyyy-MM-dd HH:mm:ss";
        public const string DateFormat = @"MM/dd/yyyy";
        public const string UniqueFileName = @"MMddyyyyHHmmss";
        public const string BPMDateFormatddMMyyyy = @"dd/MM/yyyy";
        public const string TimeFormat = @"HH:mm:ss";
        public const string GenericDateTimeFormat = @"yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";
        public const string GenericDateTimeFormatWithMiliSeconds = @"yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
        public const string AwbFormat = @"^[0-9]+$";
    }
}
