using FinTrack.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTrack.Helper;

namespace FinTrack.Services
{
    public class MenuHandler : IMenuHandler
    {
        private readonly IAuthenticationService _authService;
        public MenuHandler(IAuthenticationService authService )
        { 
            _authService = authService;
        }
        public void HandleMenuFlyoutItemClicked(object sender, MenuFlyoutItemClickedEventArgs e)
        {
           var navigationService = e.NavigationService;
            switch (e.Item.Text)
            {
                case "Logout":
                    _authService.Logout();
                    navigationService.PushAsync(new MainPage());
                    break;
                case "Exit":
                    navigationService.PushAsync(new BlazorHostPage("Counter"));
                    break;
                default: navigationService.PushAsync(new BlazorHostPage("/")); break;
            }
        }
    }
}
