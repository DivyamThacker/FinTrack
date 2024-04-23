using FinTrack.Services;
using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Mvvm.ViewModels
{//[AddINotifyPropertyChangedInterface]
    public class AccountsViewModel : INotifyPropertyChanged
    {
        private readonly RecordApiService _recordApiService;
        public ObservableCollection<RecordDTO> Records { get; set; } = new ObservableCollection<RecordDTO>();
        public IEnumerable<string> Categories { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccountsViewModel()
        {
            _recordApiService = new RecordApiService();
            Task.Run(async () => await GetRecords());
            Categories = new List<string>
            {
                "All",
                "Transport",
                "Shopping",
                "Bills",
                "Business",
                "Entertainment",
                "Health",
                "Education",
                "Other",
                "Food"
            };
        }
        private async Task GetRecords()
        {
            Records = await _recordApiService.GetDataAsync();
        }
    }
}
