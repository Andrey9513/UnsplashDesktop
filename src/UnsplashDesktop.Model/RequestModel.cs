using System;
using System.Collections.Generic;

namespace UnsplashDesktopBusinessLogic
{
    public class RequestModel
    {
        private readonly IEnumerable<string> collections;
        private readonly IEnumerable<string> features;

        public Modes Mode { get; }

        public string User { get; }

        public string Collections
        {
            get
            {
                string collectionsStr = String.Empty;
                foreach(var collection in collections)
                {
                    collectionsStr += collection + ",";
                };
                return collectionsStr.Trim(',', ' ');
            }
        }

        public string Features
        {
            get
            {
                string featuresStr = "?";
                foreach(var feature in features)
                {
                    featuresStr += feature + ",";
                }
                return featuresStr.Trim(',', ' ');
            }
        }

        public Orientations Orientation { get; set; }

        public string Size { get; set; }

        public RequestModel(Modes mode, IEnumerable<string> options = null, string user = "", string size = "", Orientations orientation = Orientations.Any)
        {
            Mode = mode;
            Orientation = orientation;
            Size = size;
            switch (mode)
            {
                case Modes.collection:
                    {
                        this.collections = options;
                        break;
                    }
                case Modes.featured:
                    {
                        this.features = options;
                        break;
                    }
                case Modes.user:
                    {
                        this.User = user;
                        break;
                    }
            };

        }
    }

    public enum Modes
    {
        user,
        collection,
        featured
    }

    public enum Orientations
    {
        Any,
        landscape,
        portrait,
        squarish
    }







}
