using System.Windows;
using System.Windows.Controls;

namespace WeatherDataAnalyser.MVVM.View;

public partial class GraphView : UserControl
{
    public GraphView()
    {
        InitializeComponent();
        DataContext = Application.Current.MainWindow?.DataContext;
    }
}