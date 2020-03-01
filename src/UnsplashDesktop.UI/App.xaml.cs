﻿using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using UnsplashDesktopBusinessLogic;
using UnsplashDesktopUI.ViewModels;

namespace UnsplashDesktopUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;

        private RequestModel DefaultRequest => new RequestModel(Modes.featured, new List<string>() { "winter" }, size: "1920x1080");

        private int DefautlTimeout => 10;

        private int DefaultImageCount => 10;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var rootPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(rootPath);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            var wallpaperManager = new WallpaperManager(DefaultRequest, DefautlTimeout, DefaultImageCount);
            notifyIcon.DataContext = new NotifyIconViewModel(wallpaperManager);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
