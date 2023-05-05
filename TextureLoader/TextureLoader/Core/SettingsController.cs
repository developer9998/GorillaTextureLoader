using System;
using System.IO;
using System.Xml.Serialization;

namespace TextureLoader.Core
{
    internal class SettingsController
    {
        internal static SettingsController settingsController;

        private string path;
        public SettingsController()
        {
            "Init : SettingsController".Log();

            settingsController = this;
            path = Path.GetDirectoryName(typeof(SettingsController).Assembly.Location) + "/settings.xml";

            if (!File.Exists(path))
                MakeFile(new Settings());
            if (GetSettings() == null)
                MakeFile(new Settings());
        }

        public void MakeFile(Settings settings)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            using (StreamWriter streamWriter = new StreamWriter(path))
                xmlSerializer.Serialize(streamWriter, settings);
        }

        private Settings GetSettings()
        {
            if (File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                using (Stream stream = File.OpenRead(path))
                    return (Settings)xmlSerializer.Deserialize(stream);
            }
            return null;
        }

        public static bool LoadOnStartup
        {
            get
            {
                return settingsController.GetSettings().LoadOnStartup;
            }
            set
            {
                Settings settings = settingsController.GetSettings();
                settings.LoadOnStartup = value;
                settingsController.MakeFile(settings);
            }
        }

        public static string SelectedKey
        {
            get
            {
                return settingsController.GetSettings().SelectedKey;
            }
            set
            {
                Settings settings = settingsController.GetSettings();
                settings.SelectedKey = value;
                settingsController.MakeFile(settings);
            }
        }
    }

    [Serializable]
    public class Settings
    {
        public bool LoadOnStartup { get; set; }
        public string SelectedKey { get; set; }
    }
}
