using System.Windows;
using System.Windows.Controls;

namespace WeatherDataAnalyser.MVVM.View;

public partial class AnalysisView : UserControl
{
    public AnalysisView()
    {
        InitializeComponent();
        DataContext = Application.Current.MainWindow?.DataContext;
    }
}