namespace Apogee.Search.Data
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using Apogee.Search.Model;

    public class Serializer
    {
        public GroupedPaths Read(string path, CommandParameters parameters)
        {
            if (!File.Exists(path))
                return new GroupedPaths();

            var xml = Deserialize<Settings>(path);
            var level = parameters.ExcludeLevel ?? xml.ExcludeLevel;
            return Paths(xml, level);
        }

        private T Deserialize<T>(string path)
        {
            using var reader = XmlReader.Create(File.OpenRead(path));
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(reader);
        }

        private GroupedPaths Paths(Settings xml, ExcludeLevel level) => level switch
        {
            ExcludeLevel.None => new GroupedPaths(),
            ExcludeLevel.Minimal => new GroupedPaths(xml.Minimal),
            ExcludeLevel.Standard => new GroupedPaths(xml.Minimal).Concat(xml.Standard),
            ExcludeLevel.Additional => new GroupedPaths(xml.Minimal).Concat(xml.Standard).Concat(xml.Additional),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, "Parameter is not a valid enumeration value.")
        };
    }
}
