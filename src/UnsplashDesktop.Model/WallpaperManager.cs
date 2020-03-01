using System.Threading;

namespace UnsplashDesktopBusinessLogic
{
    public class WallpaperManager
    {
        private readonly WindowsDesktopHelper desktopHelper;
        private int savedImageCount;

        public string ImageDirPath => desktopHelper.ImageDirPath;

        public bool IsStarted { get; private set; }

        public int SavedImageCount 
        { 
            get => savedImageCount; 
            set 
            { 
                savedImageCount = value;
                desktopHelper?.SetSavedImageCount(value);
            } 
        }        

        public RequestModel Request { get; private set; }

        public int TimeoutSec { get; set; }

        public void ChangeRequestModel(RequestModel newRequestModel)
        {
            Request = newRequestModel;
            Start();
        }

        public WallpaperManager(RequestModel request, int timeoutSec, int savedImageCount)
        {
            Request = request;
            TimeoutSec = timeoutSec;
            SavedImageCount = savedImageCount;
            desktopHelper = new WindowsDesktopHelper(savedImageCount);
        }

        public void Start()
        {
            if(!IsStarted)
            {
                IsStarted = true;
                var blThread = new Thread(() =>
                {
                    while (IsStarted)
                    {
                        if (UnslashAPIHelper.TryGetUnslashPhoto(Request, out byte[] image))
                        {
                            desktopHelper.SetDesktop(image);
                        }

                        Thread.Sleep(TimeoutSec * 1000);
                    }
                })
                {
                    Name = "Bussines logic thread"
                };
                blThread.Start();
            }
        }

        public void Stop()
        {
            IsStarted = false;
        }
    }
}
