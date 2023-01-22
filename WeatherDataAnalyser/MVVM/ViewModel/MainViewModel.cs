﻿using WeatherDataAnalyser.Core;
using WeatherDataAnalyser.MVVM.Model;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand AboutViewCommand { get; set; }
    public RelayCommand AnalysisViewCommand { get; set; }
    public RelayCommand PredictionsViewCommand { get; set; }
    private HomeViewModel HomeVm { get; set; }
    public AboutViewModel AboutVm { get; set; }
    public PredictionsViewModel? PredictionsVm { get; set; }
    public AnalysisViewModel? AnalysisVm { get; set; }
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
            AnalysisVm = new AnalysisViewModel(repository, statisticsCalc);
            CurrentView = AnalysisVm;
        });        
        
        PredictionsViewCommand = new RelayCommand(_ =>
        {
            PredictionsVm = new PredictionsViewModel(repository, statisticsCalc);
            CurrentView = PredictionsVm;
        });
        
    }
}