using System.Diagnostics;
using WeatherDataAnalyser.Core;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class AboutViewModel : ObservableObject
{
    public RelayCommand HyperlinkCommand { get; set; }

    public AboutViewModel()
    {
        HyperlinkCommand = new RelayCommand(o =>
        {
            if (o is string url) Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        });
    }
}