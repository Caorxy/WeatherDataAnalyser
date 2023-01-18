using WeatherDataAnalyser.Core;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public RelayCommand HomeViewCommand { get; set; }
    private HomeViewModel HomeVm { get; set; }
    private object? _currentView;

    public object? CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        HomeVm = new HomeViewModel();
        CurrentView = HomeVm;
        
        HomeViewCommand = new RelayCommand(_ =>
        {
            CurrentView = HomeVm;
        });
    }
}