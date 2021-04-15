using Entity.IntegrationTest.Models;
using EntityG.Contracts.Requests.Identity;
using EntityG.EntityFramework.Contexts;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Entity.IntegrationTest.Helpers
{
    public static class AccountHelper
    {
        public static ApplicationDbContext DbContext => DatabaseHelper.CreateDbContext();

        public static  Task<HttpResponseMessage> SubmitCreateAccountRequest(AccountCreatePrarams param, string token, string server = "")
        {
            var request = new RegisterRequest
            {
                UserName = param.Username,
                ActivateUser = true,
                AutoConfirmEmail = true,
                Password = param.Password,
                Email = param.Email,
                FirstName = param.FirstName,
                LastName = param.LastName,
                PhoneNumber = param.PhoneNumber,
                ConfirmPassword = param.Password
            };

            return RestApiHelper.GetHttpClient(token: token, url : server).PostAsJsonAsync("api/identity/user", request);
        }

        public static async Task<HttpResponseMessage> SubmitLoginRequest(string email, string password)
        {
            var request = new TokenRequest
            {
                Email = email,
                Password = password
            };

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new System.Uri(ConfigurationHelper.ServerUrl);

            return await RestApiHelper.GetHttpClient().PostAsJsonAsync("api/identity/token", request);
        }

        public static async Task<HttpResponseMessage> SubmitRefreshTokenRequest(string refresh_token, string access_token)
        {
            var request = new RefreshTokenRequest
            {
                RefreshToken = refresh_token,
                Token = access_token
            };

            return await RestApiHelper.GetHttpClient().PostAsJsonAsync("api/identity/token/refresh", request);
        }

        public static async Task<HttpResponseMessage> SubmitChangePasswordRequest(
            string password, 
            string newPassword, 
            string confirmPassword, 
            string access_token)
        {
            var request = new ChangePasswordRequest
            {
                Password = password,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword
            };

            return await RestApiHelper.GetHttpClient(token: access_token).PutAsJsonAsync("api/identity/account/changepassword", request);
        }

        public static bool IsRecoundFound(string username)
        {
            return DbContext.Users.Any(x => x.UserName == username);
        }
    }
}
