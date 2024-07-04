using FinTrack.Mvvm.Views;
using FinTrack.Services.IServices;
using FinTrack_Common;
using FinTrack_Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.JSInterop;


namespace FinTrack.Components.Pages.Account
{
    public partial class Account : ComponentBase
    {
        private List<AccountDTO> SavingAccounts { get; set; } = new List<AccountDTO>();
        private List<AccountDTO> InvestmentAccounts { get; set; } = new List<AccountDTO>();
        private AccountDTO SelectedAccount { get; set; }
        private UserDTO UserDetails { get; set; }
        public List<AccountGroup> AccountGroups { get; set; } = new List<AccountGroup>();
        //private ConfirmDialog confirmDialog;
        [Inject]
        private IJSRuntime JS { get; set; }
        protected override async Task OnInitializedAsync()
        {
            IsProcessing = true;
            var user = _preferences.Get<string>(SD.Local_UserDetails, null);
            if (user == null) throw new NullReferenceException();

            UserDetails = JsonConvert.DeserializeObject<UserDTO>(user);
            var allAccounts = await _accountService.GetAccounts(UserDetails.Id);
            SavingAccounts = allAccounts.Where(u => u.Type == SD.Account_Savings).ToList();
            InvestmentAccounts = allAccounts.Where(u => u.Type == SD.Account_Investment).ToList();
            AccountGroups.Add(new AccountGroup { Type = "Savings", Accounts = SavingAccounts });
            AccountGroups.Add(new AccountGroup { Type = "Investment", Accounts = InvestmentAccounts });
            IsProcessing = false;
        }

        public class AccountGroup
        {
            public string Type { get; set; }
            public List<AccountDTO> Accounts { get; set; }
        }
        private AccountCreationRequestDTO AccountCreationRequest = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowAccountCreationErrors { get; set; }
        public string Errors { get; set; }
        [Inject]
        public IAccountService _accountService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public IPreferences _preferences { get; set; }
        public string ReturnUrl { get; set; }

        private async Task OnSelectAccount(AccountDTO account)
        {
            UserDetails.AccountId = account.Id;
            var userJson = JsonConvert.SerializeObject(UserDetails);
            _preferences.Set(SD.Local_UserDetails, userJson);

            if (account.Type == SD.Account_Savings)
            {
                //_preferences.Set(SD.Local_AccountType, SD.Account_Savings);
                await App.Current.MainPage.Navigation.PushAsync(new OverviewPage());
            }
            else if (account.Type == SD.Account_Investment)
            {
                //_preferences.Set(SD.Local_AccountType, SD.Account_Investment);
                _navigationManager.NavigateTo("/dashboard");
            }
            else throw new NotImplementedException();
        }
        private async Task RefreshPage()
        {
            await JS.InvokeVoidAsync("location.reload");
        }
        private async Task DeleteBtnClicked(string accountId)
        {
            //bool isConfirmed = await confirmDialog.Show();
            bool isConfirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this item?");
            if (isConfirmed)
            {
                // Perform delete action
                var result = await _accountService.DeleteAccountAsync(accountId);
                if (result==1) await JS.InvokeVoidAsync("alert", "Item deleted!");
                else await JS.InvokeVoidAsync("alert", "Item not deleted!");
                RefreshPage();
            }
            else
            {
                await JS.InvokeVoidAsync("alert", "Delete canceled!");
            }
        }
        private async Task CreateAccount()
        {
            ShowAccountCreationErrors = false;
            IsProcessing = true;
            var result = await _accountService.CreateAccountAsync(AccountCreationRequest);
            if (result.Success)
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
                Errors = result.Message;
                ShowAccountCreationErrors = true;
            }
        }
    }
}
