using FinTrack.Mvvm.Views;
namespace FinTrack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new OverviewPage());
        }
    }
}
