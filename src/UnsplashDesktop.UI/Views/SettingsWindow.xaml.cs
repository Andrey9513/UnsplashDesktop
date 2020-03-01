using MahApps.Metro.Controls;
using Serilog;
using System;
using System.Windows;
using UnsplashDesktopUI.ViewModels;

namespace UnsplashDesktopUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        SettingsViewModel ViewModel { get; set; }

        public SettingsWindow(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
