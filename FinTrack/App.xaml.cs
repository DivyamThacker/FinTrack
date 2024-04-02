using FinTrack.Mvvm.Views;
namespace FinTrack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxcdnVVR2VeVUV/WUE=");
            MainPage = new NavigationPage(new TransactionsPage());
        }
    }
}
