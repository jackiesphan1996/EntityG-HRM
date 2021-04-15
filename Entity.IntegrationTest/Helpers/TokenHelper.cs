using Entity.IntegrationTest.Constants;
using Entity.IntegrationTest.Extensions;
using EntityG.Contracts.Responses.Identity;
using EntityG.EntityFramework.Contexts;
using EntityG.EntityFramework.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.IntegrationTest.Helpers
{
    public static class TokenHelper
    {
        private static ApplicationDbContext DbContext => DatabaseHelper.CreateDbContext();

        public static async Task<string>  GetAdministratorToken()
        {
            var tokenResponse = await GetToken("Administrator");
            
            return tokenResponse.Token;
        }

        public static async Task<TokenResponse> GetToken(string role)
        {
            var userName = $"Integration-Test-User-{role}";

            var existingRole = DbContext.Roles.SingleOrDefault(x => x.Name.Equals(role));

            if (existingRole == null)
            {
                throw new ArgumentNullException($"Role : {role} does not exist.");
            }

            var user = await GetOrCreateIfNotExist(userName, role);
            
            var response = await AccountHelper.SubmitLoginRequest(user.Email, DefaultConstants.DefaultPassword);

            return (await response.ToResult<TokenResponse>()).Data;
        }

        private static async Task<ApplicationUser> GetOrCreateIfNotExist(string userName, string role)
        {
            var userStore = new UserStore<ApplicationUser>(DbContext);

            var userManager = new UserManager<ApplicationUser>(userStore, null, new PasswordHasher<ApplicationUser>(), null, null, null, null, null, null);

            ApplicationUser user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    FirstName = "Integation",
                    LastName = "Test",
                    Email = $"{userName}@entityg.com",
                    UserName = userName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };

                await userManager.CreateAsync(user, DefaultConstants.DefaultPassword);
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }

            return user;
        }
    }
}
