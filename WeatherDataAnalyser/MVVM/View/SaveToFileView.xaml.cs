using System.Windows;
using System.Windows.Controls;

namespace WeatherDataAnalyser.MVVM.View;

public partial class PredictionsView : UserControl
{
    public PredictionsView()
    {
        InitializeComponent();
        DataContext = Application.Current.MainWindow?.DataContext;
    }
}