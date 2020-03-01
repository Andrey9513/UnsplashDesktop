using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using UnsplashDesktopBusinessLogic;
using UnsplashDesktopUI.Views;

namespace UnsplashDesktopUI.ViewModels
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    public class NotifyIconViewModel
    {
        public WallpaperManager Model { get; private set; }

        public NotifyIconViewModel(WallpaperManager model)
        {
            Model = model;
        }

        /// <summary>
        /// Start downloading images from Unsplash
        /// </summary>
        public ICommand StartUDCommand
        {
            get =>
                new DelegateCommand
                {
                    CanExecuteFunc = () => !(Model.IsStarted),
                    CommandAction = (p) =>
                    {
                        Model.Start();
                    }
                };
        }

        /// <summary>
        /// Stop downloading images from Unsplash
        /// </summary>
        public ICommand StopUDCommand
        {
            get=>
                new DelegateCommand
                {
                    CommandAction = (p) => Model.Stop(),
                    CanExecuteFunc = () => Model.IsStarted
                };           
        }


        /// <summary>
        /// Open folders with downloaded images
        /// </summary>
        public ICommand OpenImageFolderCommand
        {
            get => new DelegateCommand
            {
                CommandAction = (p) => {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        Arguments = Model.ImageDirPath,
                        FileName = "explorer.exe"
                    };
                    Process.Start(startInfo);
                }
            };
        }

        /// <summary>
        /// Open folders with downloaded images
        /// </summary>
        public ICommand ShutdownCommand
        {
            get => new DelegateCommand
            {
                CommandAction = (p) => {
                    Environment.Exit(0);
                }
            };
        }

        /// <summary>
        /// Open settings window
        /// </summary>
        public ICommand OpenSettingsWindowCommand
        {
            get =>
                new DelegateCommand
                {
                    CommandAction = (p) =>
                    {
                        Application.Current.MainWindow = new SettingsWindow(new SettingsViewModel(Model));
                        Application.Current.MainWindow.Show();
                    },
                    CanExecuteFunc = () => Application.Current.MainWindow == null ? true : Application.Current.MainWindow.IsActive
                };
        }
    }
}
