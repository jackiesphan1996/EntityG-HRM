using AntDesign;
using EntityG.Client.Models;
using EntityG.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Identity;

namespace EntityG.Client.Pages.User
{
    public partial class Login
    {
        private readonly LoginParamsType _model = new LoginParamsType
        {
            UserName = "hoang.nhat.phan@entityg.com",
            Password = "123456"
        };

        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IAccountService AccountService { get; set; }

        [Inject] public MessageService Message { get; set; }

        public async Task HandleSubmit()
        {
            var result = await _authenticationManager.Login(new TokenRequest()
            {
                Email = _model.UserName,
                Password = _model.Password
            });
            if (result.Succeeded)
            {
                _navigationManager.NavigateTo("/", true);
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        public async Task GetCaptcha()
        {
            var captcha = await AccountService.GetCaptchaAsync(_model.Mobile);
            await Message.Success($"获取验证码成功！验证码为：{captcha}");
        }
    }
}