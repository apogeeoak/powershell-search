namespace Apogee.Search.Model
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Settings")]
    public class Settings
    {
        public ExcludeLevel ExcludeLevel { get; set; } = ExcludeLevel.None;
        public Group Minimal { get; set; } = new Group();
        public Group Standard { get; set; } = new Group();
        public Group Additional { get; set; } = new Group();
    }

    public class Group
    {
        public Paths Exact { get; set; } = new Paths();
        public Paths Partial { get; set; } = new Paths();
    }

    public class Paths
    {
        public string[] Directories { get; set; } = Array.Empty<string>();
        public string[] Files { get; set; } = Array.Empty<string>();
    }
}
