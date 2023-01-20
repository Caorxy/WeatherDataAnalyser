﻿using System.Windows;
using System.Windows.Controls;

namespace WeatherDataAnalyser.MVVM.View;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        DataContext = Application.Current.MainWindow?.DataContext;
    }
}