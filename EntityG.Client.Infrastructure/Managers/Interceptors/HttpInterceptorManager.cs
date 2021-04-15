using EntityG.Client.Infrastructure.Managers.Identity.Authentication;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AntDesign;
using Toolbelt.Blazor;

namespace EntityG.Client.Infrastructure.Managers.Interceptors
{
    public class HttpInterceptorManager : IHttpInterceptorManager
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly NavigationManager _navigationManager;
        private readonly MessageService _messageService;
        public HttpInterceptorManager(
            HttpClientInterceptor interceptor, 
            IAuthenticationManager authenticationManager, 
            NavigationManager navigationManager,
            MessageService messageService
            )
        {
            _interceptor = interceptor;
            _authenticationManager = authenticationManager;
            _navigationManager = navigationManager;
            _messageService = messageService;
        }
        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (!absPath.Contains("token") && !absPath.Contains("accounts"))
            {
                try
                {
                    var token = await _authenticationManager.TryRefreshToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        await _messageService.Success("Refreshed Token.");
                        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await _authenticationManager.Logout();
                    _navigationManager.NavigateTo("/");
                    await _messageService.Error("You are Logged Out.");
                }

            }
        }
        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}
