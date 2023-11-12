using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UnsplashDesktopBusinessLogic;
using UnsplashDesktopUI.Views;

namespace UnsplashDesktopUI.ViewModels
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    public class NotifyIconViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public WallpaperManager Model { get; private set; }
        public TaskbarIcon Icon {get; private set;}

        public NotifyIconViewModel(WallpaperManager model, TaskbarIcon icon)
        {
            Model = model;
            Icon = icon;
            Model.Start();
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
                        Icon.Icon = new System.Drawing.Icon(Path.Combine(Directory.GetCurrentDirectory(),"Resources\\u_green.ico"));
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
                    CommandAction = (p) => {
                        Model.Stop();
                        Icon.Icon = new System.Drawing.Icon(Path.Combine(Directory.GetCurrentDirectory(), "Resources\\u_red.ico"));
                    },
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
                        Application.Current.MainWindow = new SettingsWindow(new SettingsViewModel(Model,this));
                        Application.Current.MainWindow.Show();
                    },
                    CanExecuteFunc = () => Application.Current.MainWindow == null ? true : Application.Current.MainWindow.IsActive
                };
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
