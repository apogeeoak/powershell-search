namespace Apogee.Search.Configuration
{
    using System;

    public class Configuration
    {
        public Settings Settings { get; } = new Settings();
    }

    public class Settings
    {
        public Settings()
        {
            var assemblyUri = new Uri(typeof(Settings).Assembly.CodeBase);
            SettingsPath = new Uri(assemblyUri, settingsFile).LocalPath;
        }

        private string settingsFile = "settings.xml";

        public string SettingsPath { get; }
    }
}
