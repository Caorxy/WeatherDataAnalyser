using WeatherDataAnalyser.Core;
using WeatherDataAnalyser.MVVM.Model;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand AboutViewCommand { get; set; }
    public RelayCommand AnalysisViewCommand { get; set; }
    private HomeViewModel HomeVm { get; set; }
    private AboutViewModel AboutVm { get; set; }
    public AnalysisViewModel AnalysisVm { get; set; }
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
        IRepository repository = new Repository();
        IStatisticsCalc statisticsCalc = new StatisticsCalc();
        HomeVm = new HomeViewModel();
        AboutVm = new AboutViewModel();
        AnalysisVm = new AnalysisViewModel(repository, statisticsCalc);
        CurrentView = HomeVm;
        
        HomeViewCommand = new RelayCommand(_ =>
        {
            CurrentView = HomeVm;
        });
        
        AboutViewCommand = new RelayCommand(_ =>
        {
            CurrentView = AboutVm;
        });        
        
        AnalysisViewCommand = new RelayCommand(_ =>
        {
            CurrentView = AnalysisVm;
        });
    }
}