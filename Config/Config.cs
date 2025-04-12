namespace StoreManageAPI.Config
{
    public class Config
    {
        public static string Issues { get; } = "JWT:Issuer";
        public static string Audience { get; } = "JWT:Audience";
        public static string AccessTokenSecret { get; } = "JWT:AccessTokenSecret";
        public static string RefreshTokenSecret { get; } = "JWT:RefreshTokenSecret";
        public static string AccessTokenExpireMinutes { get; } = "JWT:AccessTokenExpireMinutes";
        public static string RefreshTokenExpireMinutes { get; } = "JWT:RefreshTokenExpireMinutes";

        public static string AllowedOrigins { get; } = "AllowedOrigins";

        public static string PolicyName {get; } = "CORS";

        public static int CodeConfirmExpire { get; } = 2;

        public static string RefreshTokenCookiesName { get; } = "refreshToken";

        // cloudinary
        public const string CloudName = "Cloudinary:CloudName";
        public const string ApiKey = "Cloudinary:ApiKey";
        public const string ApiSecret = "Cloudinary:ApiSecret";


        // config quantity result
        public static int DEFAULT_PAGE_SIZE { get; } =  10;
        public static int DEFAULT_PAGE_INDEX { get; } = 1;
        public static int DEFAULT_SEARCH_RESULT { get; } = 10;

    }
}
