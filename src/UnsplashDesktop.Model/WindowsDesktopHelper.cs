using Microsoft.Win32;
using Serilog;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

namespace UnsplashDesktopBusinessLogic
{
    public class WindowsDesktopHelper
    {
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 0x01;
        public const int SPIF_SENDWININICHANGE = 0x02;
        public const string ImageFileName = "wallpaper";
        private int count;
        private int savedImageCount;
        
        public string ImageDirPath { get; private set; }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        public WindowsDesktopHelper(int savedImageCount)
        {
            count = 0;
            ImageDirPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(ImageDirPath))
            {
                Directory.CreateDirectory(ImageDirPath);
            }
             SetSavedImageCount(savedImageCount);
        }

        public WindowsDesktopHelper()
        {
            count = 0;
            ImageDirPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(ImageDirPath))
            {
                Directory.CreateDirectory(ImageDirPath);
            }
            SetSavedImageCount(1);
        }

        public void SetSavedImageCount(int count)
        {
            savedImageCount = count;
            if (!Directory.Exists(ImageDirPath))
            {
                Directory.CreateDirectory(ImageDirPath);
            }
            foreach (var fileStr in Directory.GetFiles(ImageDirPath))
            {
                var file = new FileInfo(fileStr);
                var number = int.Parse(file.Name.Remove(file.Name.Length - 4).Substring(ImageFileName.Length));
                if(number>= count)
                {
                    file.Delete();
                }
            } 
        }

        public void SetDesktop(byte[] image)
        {
            try
            {

                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", $"wallpaper{count}.jpg");
                if(!Directory.Exists(Path.GetDirectoryName(imagePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
                }
                File.WriteAllBytes(imagePath, image);
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());

                int result = SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0,
                    imagePath,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

                count++;
                count %= savedImageCount;

            }
            catch (Exception exc)
            {
                Log.Error(exc, "It is not possible to install desktop wallpaper");
            }

        }
    }
}
