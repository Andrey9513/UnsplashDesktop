using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using UnsplashDesktopBusinessLogic;

namespace UnsplashDesktopUI.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        #region Constants

        private const string AnySizeStr = "Any";

        #endregion

        #region Enums
        public enum Units
        {
            second,
            minute,
            hour,
            week
        }
        #endregion

        #region Fields

        private int timeout;
        private string optionsStr;
        private Modes selectedMode;
        private Orientations selectedOrientation;
        private string selectedSize;
        private int selectedImageCount;

        #endregion

        #region NotifiedProperties

        public string OptionsStr
        {
            get => optionsStr;
            set
            {
                optionsStr = value;
                OnPropertyChanged(nameof(OptionsStr));
            }
        }

        public int SelectedTimeout
        {
            get => timeout;
            set
            {
                timeout = value;
                OnPropertyChanged(nameof(SelectedTimeout));
            }
        }

        public Modes SelectedMode
        {
            get => selectedMode;
            set
            {
                selectedMode = value;
                OnPropertyChanged(nameof(SelectedMode));
            }
        }

        public Orientations SelectedOrientation
        {
            get => selectedOrientation;
            set
            {
                selectedOrientation = value;
                OnPropertyChanged(nameof(SelectedOrientation));
            }
        }

        public string SelectedSize
        {
            get => selectedSize;
            set
            {
                selectedSize = value;
                OnPropertyChanged(nameof(SelectedOrientation));
            }
        }

        public int SelectedImageCount
        {
            get => selectedImageCount;
            set
            {
                selectedImageCount = value;
                OnPropertyChanged(nameof(SelectedImageCount));
    }
       }


        #endregion

        #region SimpleProperties

        public IEnumerable<Modes> RequestModes => Enum.GetValues(typeof(Modes)).Cast<Modes>();

        public IEnumerable<string> ImageSizes  => new List<string>()
            {
                "1920x1080",
                "1680x1050",
                "1600x900",
                "1440x900",
                "1400x1050",
                "1366x768",
                "1360x768",
                "1280x960",
                "1280x800",
                "1280x768",
                "1280x720",
                "1280x600",
                "1152x864",
                "1024x768",
                "800x600",
                "Any"
            };

        public IEnumerable<Orientations> ImageOrientations => Enum.GetValues(typeof(Orientations)).Cast<Orientations>();

        public IEnumerable<string> Timeouts => new List<string>()
            {
                "10 second",
                "1 minute",
                "10 minute",
                "15 minute",
                "20 minute",
                "30 minute",
                "1 hour",
                "1,5 hour",
                "2 hour",
                "3 hour",
                "6 hour",
                "12 hour",
                "1 day",
                "2 day",
                "3 day",
                "1 week"
            };

        public IEnumerable<string> ImageCounts => new List<string>()
        {
            "1","5","10","25","50","100"
        };

        public WallpaperManager Model { get; set; }

        #endregion

        #region Commands

        public ICommand ApplyCommand
        {
            get =>
                new DelegateCommand
                {
                    CanExecuteFunc = () => !(Model is null),
                    CommandAction = (p) =>
                    {
                        var request = SelectedMode switch
                        {
                            Modes.user => SelectedSize == "Any" ? new RequestModel(Modes.user, options:null, user: OptionsStr, size:string.Empty, orientation: SelectedOrientation) :
                                                                 new RequestModel(Modes.user, options: null, user: OptionsStr, size:SelectedSize, orientation: SelectedOrientation),
                            _=> SelectedSize == "Any" ? new RequestModel(SelectedMode, options: OptionsStr.Split(','), size: string.Empty, orientation: SelectedOrientation) :
                                      new RequestModel(SelectedMode, options: OptionsStr.Split(','), size: SelectedSize, orientation: SelectedOrientation),
                        };
                        Model.TimeoutSec = SelectedTimeout;
                        Model.SavedImageCount = SelectedImageCount;
                        Model.ChangeRequestModel(request);
                    }
                };
        }

        public ICommand CancelCommand
        {
            get => new DelegateCommand
                   {
                        CanExecuteFunc = () => !(Model is null),
                        CommandAction = (p) =>
                        {
                            ((Window)p).Close();
                        }
                    };
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public SettingsViewModel(WallpaperManager model)
        {
            Model = model;
            SelectedMode = model.Request.Mode;
            SelectedOrientation = model.Request.Orientation;
            SelectedSize = model.Request.Size;
            SelectedTimeout = model.TimeoutSec;
            SelectedImageCount = model.SavedImageCount;
            OptionsStr = model.Request.Mode switch
            {
                Modes.user => model.Request.User,
                Modes.collection => model.Request.Collections,
                Modes.featured => model.Request.Features.Trim('?'),
                _=>model.Request.User
            };
            
        }
        
        #endregion

        #region Methods

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

       





    }
}
