using FinTrack.Mvvm.Views;
using FinTrack.Services.IServices;
using FinTrack_Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FinTrack.Components.Pages.Account
{
    public partial class Login
    {
        private SignInRequestDTO SignInRequest = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowSignInErrors { get; set; }
        public string Errors { get; set; }
        //[Inject]
        //public IBudgetApiService budgetApiService { get; set; }
        [Inject]
        IGoalApiService goalApiService { get; set; }
        [Inject]
        public IRecordApiService recordApiService { get; set; }
        [Inject]
        public ITransactionApiService transactionApiService { get; set; }

        [Inject]
        public IAuthenticationService _authSerivce { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        public string ReturnUrl { get; set; }

        private async Task LoginUser()
        {
            ShowSignInErrors = false;
            IsProcessing = true;
            var result = await _authSerivce.Login(SignInRequest);
            if (result.IsAuthSuccessful)
            {
                //regiration is successful
                var absoluteUri = new Uri(_navigationManager.Uri);
                var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
                ReturnUrl = queryParam["returnUrl"];
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    //_navigationManager.NavigateTo("/");
                    await App.Current.MainPage.Navigation.PushAsync(new OverviewPage());

                }
                else
                {
                    _navigationManager.NavigateTo("/" + ReturnUrl);
                }
            }
            else
            {
                //failure
                Errors = result.ErrorMessage;
                ShowSignInErrors = true;

            }
            IsProcessing = false;
        }
    }
}