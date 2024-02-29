namespace NotificationBackendService
{
    public class Constants
    {
        public class Jwt
        {
            public const string SecretKey = "Jwt:SecretKey";
            public const string AudienceName = "Jwt:Audience";
            public const string IssuerName = "Jwt:Issuer";
            public const string ExpiryDurationMinsName = "Jwt:ExpiryDurationMins";
            public const string TokenHeaderKIDName = "kid";
        }

        public class JwtClaims
        {
            public const string IsBlock = "IsBlocked";
        }

        public const string HeaderTenantId = "X-Tenant-Id";
        public const string HeaderAPIKey = "X-API-Key";

        public const string QueryStringApiVersionName = "api-version";
        public const string HeaderApiVersionName = "X-Version";
        public const string MediaTypeApiVersionNane = "ver";

    }
}
