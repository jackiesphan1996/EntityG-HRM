using Entity.IntegrationTest.Builders;
using Entity.IntegrationTest.Extensions;
using Entity.IntegrationTest.Helpers;
using Entity.IntegrationTest.Models;
using EntityG.Contracts.Responses.Identity;
using EntityG.Shared.Wrapper;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Entity.IntegrationTest.Scenarios.Identity.Account
{
    [TestFixture]
    public class AccountTest
    {
        private string _token;

        [SetUp]
        public async Task SetUp()
        {
            _token = await TokenHelper.GetAdministratorToken(); 
        }

        [Test]
        public async Task Login_Successfully()
        {
            // Arrange
            AccountCreatePrarams accountParams = new AccountBuilder().Build();

            // Act
            // Create basic user
            HttpResponseMessage createAccountResponse = await AccountHelper.SubmitCreateAccountRequest(accountParams, _token);
            createAccountResponse.EnsureSuccessStatusCode();
            var isRecordFound = AccountHelper.IsRecoundFound(accountParams.Username);

            // Login
            HttpResponseMessage loginResponse = await AccountHelper.SubmitLoginRequest(accountParams.Email, accountParams.Password);
            createAccountResponse.EnsureSuccessStatusCode();
            IResult<TokenResponse> tokenInfo = await loginResponse.ToResult<TokenResponse>();

            // Assert
            Assert.IsTrue(isRecordFound);
            Assert.IsNotNull(tokenInfo);
            Assert.IsTrue(tokenInfo.Succeeded);
            Assert.IsNotNull(tokenInfo.Data);
            Assert.IsNotNull(tokenInfo.Data.Token);
        }

        [Test]
        public async Task Login_With_Not_Found_User()
        {
            // Arrange
            var email = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            // Act
            HttpResponseMessage loginResponse = await AccountHelper.SubmitLoginRequest(email, password);
            loginResponse.EnsureSuccessStatusCode();
            IResult<TokenResponse> tokenInfo = await loginResponse.ToResult<TokenResponse>();

            // Assert
            Assert.IsNotNull(tokenInfo);
            Assert.IsNull(tokenInfo.Data);
            Assert.IsFalse(tokenInfo.Succeeded);
            Assert.AreEqual(tokenInfo.Messages[0], "User Not Found.");
        }

        [Test]
        public async Task Login_With_Invalid_Credentials()
        {
            // Arrange
            AccountCreatePrarams accountParams = new AccountBuilder().Build();
            string wrongPassword = Guid.NewGuid().ToString();

            // Act
            // Create basic user
            HttpResponseMessage createAccountResponse = await AccountHelper.SubmitCreateAccountRequest(accountParams, _token);
            createAccountResponse.EnsureSuccessStatusCode();
            var isRecordFound = AccountHelper.IsRecoundFound(accountParams.Username);

            HttpResponseMessage loginResponse = await AccountHelper.SubmitLoginRequest(accountParams.Email, wrongPassword);
            loginResponse.EnsureSuccessStatusCode();
            IResult<TokenResponse> tokenInfo = await loginResponse.ToResult<TokenResponse>();

            // Assert
            Assert.IsTrue(isRecordFound);
            Assert.IsNotNull(tokenInfo);
            Assert.IsNull(tokenInfo.Data);
            Assert.IsFalse(tokenInfo.Succeeded);
            Assert.AreEqual(tokenInfo.Messages[0], "Invalid Credentials.");
        }

        [Test]
        public async Task RefreshToken_Successfully()
        {
            // Arrange
            AccountCreatePrarams accountParams = new AccountBuilder().Build();

            // Act
            // Create basic user
            HttpResponseMessage createAccountResponse = await AccountHelper.SubmitCreateAccountRequest(accountParams, _token);
            createAccountResponse.EnsureSuccessStatusCode();
            var isRecordFound = AccountHelper.IsRecoundFound(accountParams.Username);

            // Login
            HttpResponseMessage loginResponse = await AccountHelper.SubmitLoginRequest(accountParams.Email, accountParams.Password);
            createAccountResponse.EnsureSuccessStatusCode();
            IResult<TokenResponse> tokenInfo = await loginResponse.ToResult<TokenResponse>();

            // Refresh token
            HttpResponseMessage refreshTokenResponse = await AccountHelper.SubmitRefreshTokenRequest(tokenInfo.Data.RefreshToken, tokenInfo.Data.Token);
            refreshTokenResponse.EnsureSuccessStatusCode();
            IResult<TokenResponse> refreshToken = await loginResponse.ToResult<TokenResponse>();

            // Assert
            Assert.IsTrue(isRecordFound);
            Assert.IsNotNull(tokenInfo);
            Assert.IsNotNull(refreshToken);
            Assert.IsTrue(refreshToken.Succeeded);
            Assert.IsNotNull(refreshToken.Data);
            Assert.IsNotNull(refreshToken.Data.Token);
            Assert.IsNotNull(refreshToken.Data.RefreshToken);
        }

        [Test]
        public async Task ChangePassword_Successfully()
        {
            // Arrange
            AccountCreatePrarams accountParams = new AccountBuilder().Build();

            // Act
            // Create basic user
            HttpResponseMessage createAccountResponse = await AccountHelper.SubmitCreateAccountRequest(accountParams, _token);
            createAccountResponse.EnsureSuccessStatusCode();
            var isRecordFound = AccountHelper.IsRecoundFound(accountParams.Username);

            // Login
            HttpResponseMessage loginResponse = await AccountHelper.SubmitLoginRequest(accountParams.Email, accountParams.Password);
            createAccountResponse.EnsureSuccessStatusCode();
            IResult<TokenResponse> tokenInfo = await loginResponse.ToResult<TokenResponse>();

            // Change password
            string newPassword = "New_Pass_Word";

            HttpResponseMessage changePasswordResponse = await AccountHelper.SubmitChangePasswordRequest(
                accountParams.Password, 
                newPassword, newPassword, 
                tokenInfo.Data.Token);

            changePasswordResponse.EnsureSuccessStatusCode();

            // Login again with new password
            loginResponse = await AccountHelper.SubmitLoginRequest(accountParams.Email, newPassword);
            createAccountResponse.EnsureSuccessStatusCode();
            var newTokenInfo = await loginResponse.ToResult<TokenResponse>();

            // Assert
            Assert.IsNotNull(newTokenInfo);
            Assert.IsTrue(newTokenInfo.Succeeded);
            Assert.IsNotNull(newTokenInfo.Data);
            Assert.IsNotNull(newTokenInfo.Data.Token);
            Assert.IsNotNull(newTokenInfo.Data.RefreshToken);
        }

        private async Task<HttpResponseMessage> Do(int i)
        {
            // Arrange
            AccountCreatePrarams accountParams = new AccountBuilder().WithFirstName(i.ToString()).Build();

            Random random = new Random();

            var servers = new string[]
            {
                "http://steven.southeastasia.cloudapp.azure.com:10000",
                "https://blazor-hero.azurewebsites.net"
            };

            // Act
            // Create basic user
            await Task.Yield();
            return await  AccountHelper.SubmitCreateAccountRequest(accountParams, _token, servers[random.Next(0, servers.Length)]);
           
        }
    }
}
