using FinTrack.IViews;
using FinTrack.Mvvm.Views;
using FinTrack.Mvvm.Views.MobileViews;
using FinTrack.Services;
namespace FinTrack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxcdnVVR2VeVUV/WUE=");

            //MainPage = new NavigationPage(ViewServices.ResolvePage<IStartPage>());
            MainPage = new NavigationPage(new MainPage());

            //#if ANDROID || IOS
            //                MainPage = new NavigationPage(new MOverViewPage());
            //#else
            //            MainPage = new NavigationPage(new OverviewPage());
            //#endif


            //MainPage = new NavigationPage(new OverviewPage());
        }
    }
}
