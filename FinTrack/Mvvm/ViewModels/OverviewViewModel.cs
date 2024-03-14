using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PropertyChanged;
using System.Diagnostics;
using System.Windows.Input;

namespace FinTrack.Mvvm.ViewModels;

[AddINotifyPropertyChangedInterface]
public class OverviewViewModel : ObservableObject
{

    public OverviewViewModel()
	{
        
    }
}
