
namespace EntityG.Client.Infrastructure.Routes
{
    public static class UserEndpoint
    {
        public static string GetAllWithPaging(int page, int pageSize, string search) => $"api/identity/user?page={page}&pageSize={pageSize}&search={search}";

        public static string GetAll => $"api/identity/user/lookup";

        public static string Get(string userId)
        {
            return $"api/identity/user/{userId}";
        }

        public static string GetUserRoles(string userId)
        {
            return $"api/identity/user/roles/{userId}";
        }

        public static string Register = "api/identity/user";
        public static string ToggleUserStatus = "api/identity/user/toggle-status";
        public static string ForgotPassword = "api/identity/user/forgot-password";
        public static string ResetPassword = "api/identity/user/reset-password";
    }
}