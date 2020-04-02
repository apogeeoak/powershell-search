namespace Apogee.Search.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apogee.Search.Configuration;
    using Apogee.Search.Data;
    using Apogee.Search.Model;

    public class SettingsController
    {
        public static SettingsController Create(CommandParameters parameters)
        {
            var settingsPath = new Configuration().Settings.SettingsPath;
            var paths = new Serializer().Read(settingsPath, parameters);
            return new SettingsController(paths);
        }

        private SettingsController(GroupedPaths paths)
        {
            exactDirectories = CreateHashSet(paths.ExactDirectories);
            partialDirectories = paths.PartialDirectories.ToArray();
            exactFiles = CreateHashSet(paths.ExactFiles);
            partialFiles = paths.PartialFiles.ToArray();
        }

        private HashSet<string> exactDirectories;
        private string[] partialDirectories;
        private HashSet<string> exactFiles;
        private string[] partialFiles;

        public bool ExcludeDirectory(string path) =>
            Exclude(exactDirectories, path) ||
            Exclude(partialDirectories, path);

        public bool ExcludeFile(string path) =>
            Exclude(exactFiles, path) ||
            Exclude(partialFiles, path);

        private bool Exclude(HashSet<string> collection, string path) => collection.Contains(path);

        private bool Exclude(string[] collection, string path) =>
            Array.Exists(collection, item => path.StartsWith(item, StringComparison.OrdinalIgnoreCase));

        private static HashSet<string> CreateHashSet(IEnumerable<string> collection) =>
            new HashSet<string>(collection, StringComparer.OrdinalIgnoreCase);
    }
}
