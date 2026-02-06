using CommunityToolkit.Mvvm.ComponentModel;

namespace HackGpon.MA5671A.Eeprom.Avalonia.ViewModels;

/// <summary>
/// Root ViewModel that manages navigation between views
/// </summary>
public partial class MainWindowModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject currentViewModel = null!;

    public MainWindowModel()
    {
        CurrentViewModel = new MainViewModel(this);
    }
}
