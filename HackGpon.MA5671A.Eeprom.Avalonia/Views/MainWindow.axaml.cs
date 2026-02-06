using Avalonia.Controls;

namespace HackGpon.MA5671A.Eeprom.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new ViewModels.MainWindowModel();
    }
}