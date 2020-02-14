namespace Apogee.Search
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class Settings
    {
        private static string settingsFile = "settings.xml";
        private static Uri assemblyUri = new Uri(typeof(Settings).Assembly.CodeBase);
        private static string settingsPath = new Uri(assemblyUri, settingsFile).LocalPath;

        private Model.Settings settings = new Model.Settings();

        public void Read(ExcludeLevel? parameterLevel)
        {
            settings = Deserialize<Model.Settings>(settingsPath);
            settings.PostProcess(parameterLevel);
        }

        public bool ExcludeFolder(string path) => settings.ExcludeFolder(path);
        public bool ExcludeFile(string path) => settings.ExcludeFile(path);

        private T Deserialize<T>(string path)
        {
            using var reader = XmlReader.Create(File.OpenRead(path));
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(reader);
        }
    }
}
