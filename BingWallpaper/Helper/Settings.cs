using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper
{
    public class Settings
    {
        private Options _options;
        private string _settingsPath;

        public enum Resolution
        {
            _1920x1200,
            _1920x1080,
            _1366x768,
            _1280x768,
            _1280x720,
            _1080x1920,
            _1024x768
        }

        public Dictionary<string, string> Country = new Dictionary<string, string>()
        {
            {"United States", "en-us"},
            {"United Kingdom", "en-gb"},
            {"Canada (English)", "en-ca"},
            {"Canada (French)", "fr-ca"},
            {"Australia", "en-au"},
            {"Germany", "de-de"},
            {"India", "en-in"},
            {"France", "fr-fr"},
            {"Japan", "ja-jp"},
            {"China", "zh-cn"},
            {"International / Other", "en-ww"}
        };

        public Settings()
        {
            _settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.txt");

            try
            {
                using (var stream = new FileStream(_settingsPath, FileMode.Open))
                {
                    var ser = new DataContractJsonSerializer(typeof(Options));
                    _options = (Options)ser.ReadObject(stream);
                }
            }
            catch (FileNotFoundException)
            {
                _options = new Options();
                Save();
            }
            catch (SerializationException)
            {
                _options = new Options();
                Save();
            }
        }

        #region User settings

        public bool LaunchOnStartup
        {
            get { return _options.LaunchOnStartup; }
            set
            {
                _options.LaunchOnStartup = value;
                Save();
            }
        }

        public string ImageCopyright
        {
            get { return _options.ImgCopyright; }
            set
            {
                _options.ImgCopyright = value;
                Save();
            }
        }

        public string ImageCopyrightLink
        {
            get { return _options.ImgCopyrightLink; }
            set
            {
                _options.ImgCopyrightLink = value;
                Save();
            }
        }

        public Resolution ImageResolution
        {
            get => (Resolution) Enum.Parse(typeof(Resolution), "_" + _options.ImgResolution); 
            set
            {
                _options.ImgResolution = value.ToString().TrimStart('_');
                Save();
            }
        }

        public string ImageCountry
        {
            get { return _options.ImgCountry; }
            set
            {
                _options.ImgCountry = value;
                Save();
            }
        }

        #endregion

        private void Save()
        {
            using (var stream = new FileStream(_settingsPath, FileMode.Create))
            {
                var ser = new DataContractJsonSerializer(typeof(Options));
                ser.WriteObject(stream, _options);
            }
        }

        [DataContract]
        private class Options
        {
            [DataMember]
            public bool LaunchOnStartup = true;
            [DataMember]
            public string ImgCopyright = "Bing Wallpaper";
            [DataMember]
            public string ImgCopyrightLink = "https://www.bing.com";
            [DataMember]
            public String ImgResolution = "1920x1080";
            [DataMember]
            public String ImgCountry = "United States";
        }
    }
}
