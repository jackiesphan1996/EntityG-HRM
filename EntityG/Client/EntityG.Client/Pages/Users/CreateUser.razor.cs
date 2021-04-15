using AntDesign;
using MudBlazor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace EntityG.Client.Pages.Users
{
    public partial class CreateUser
    {
        private bool _success;

        private string[] _errors = { };

        private Form<RegisterRequest> form;

        private RegisterRequest Model = new RegisterRequest();

        [Parameter] public EventCallback<MouseEventArgs> HandleOkay { get; set; }

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private MudTextField<string> pwField;

        private string PasswordMatch(string arg)
        {
            if (pwField.Value != arg)
                return "Passwords don't match";
            return null;
        }


        private async Task HandleSubmit()
        {
            this.form.Validate();
            if (this.form.Validate())
            {
                var response = await _userManager.RegisterUserAsync(Model);
                if (response.Succeeded)
                {
                    await HandleOkay.InvokeAsync();
                    this.form.Reset();
                    await _message.Success(response.Messages[0]);
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        await _message.Error(message);
                    }
                }
            }
        }
    }
}