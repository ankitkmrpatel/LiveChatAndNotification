using System.ComponentModel;
using System.Security.Claims;

namespace NotificationBackendService.Services
{
    public class CurrentUserIdentity : ICurrentUserIdentity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserIdentity(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            UserId = GetValueFromUserIdentity<Guid>(ClaimTypes.NameIdentifier);
            UserName = GetValueFromUserIdentity(ClaimTypes.PrimarySid);
            Email = GetValueFromUserIdentity(ClaimTypes.Email);
            Name = GetValueFromUserIdentity(ClaimTypes.Name);
            IsBlocked = GetValueFromUserIdentity<bool>(Constants.JwtClaims.IsBlock);
            AccessCode = GetValueFromUserIdentity<Guid>(ClaimTypes.Sid);
        }

        private T? GetValueFromUserIdentity<T>(string claimName)
        {
            var value = _httpContextAccessor.HttpContext?.User
                 .FindFirstValue(claimName);

            return value != null ? value.ChangeType<T>() : default;
        }

        private string GetValueFromUserIdentity(string claimName)
        {
            var value = _httpContextAccessor.HttpContext?.User
                 .FindFirstValue(claimName);

            value ??= "";

            return value;
        }

        public Guid UserId { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Name { get; init; }
        public bool IsBlocked { get; init; }
        public Guid AccessCode { get; init; }
    }
}

public static class TConverter
{
    public static T ChangeType<T>(this object value)
    {
        return (T)ChangeType(typeof(T), value);
    }

    public static object ChangeType(Type t, object value)
    {
        TypeConverter tc = TypeDescriptor.GetConverter(t);
        return tc.ConvertFrom(value);
    }

    public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
    {

        TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
    }
}
