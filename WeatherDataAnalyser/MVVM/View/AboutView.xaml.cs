using System.Windows;
using System.Windows.Controls;

namespace WeatherDataAnalyser.MVVM.View;

public partial class AboutView : UserControl
{
    public AboutView()
    {
        InitializeComponent();
        DataContext = Application.Current.MainWindow?.DataContext;
    }
}