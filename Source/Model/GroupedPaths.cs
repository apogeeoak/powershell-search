namespace Apogee.Search.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GroupedPaths
    {
        public GroupedPaths() : this(Empty, Empty, Empty, Empty) { }

        public GroupedPaths(Group group)
        {
            ExactDirectories = Expand(group.Exact.Directories);
            PartialDirectories = Expand(group.Partial.Directories);
            ExactFiles = Expand(group.Exact.Files);
            PartialFiles = Expand(group.Partial.Files);
        }

        public GroupedPaths(IEnumerable<string> exactDirectories, IEnumerable<string> partialDirectories, IEnumerable<string> exactFiles, IEnumerable<string> partialFiles)
        {
            ExactDirectories = exactDirectories;
            PartialDirectories = partialDirectories;
            ExactFiles = exactFiles;
            PartialFiles = partialFiles;
        }

        public IEnumerable<string> ExactDirectories { get; }
        public IEnumerable<string> PartialDirectories { get; }
        public IEnumerable<string> ExactFiles { get; }
        public IEnumerable<string> PartialFiles { get; }

        public GroupedPaths Concat(Group group) =>
            this.Concat(new GroupedPaths(group));

        public GroupedPaths Concat(GroupedPaths paths) => new GroupedPaths(
            exactDirectories: this.ExactDirectories.Concat(paths.ExactDirectories),
            partialDirectories: this.PartialDirectories.Concat(paths.PartialDirectories),
            exactFiles: this.ExactFiles.Concat(paths.ExactFiles),
            partialFiles: this.PartialFiles.Concat(paths.PartialFiles)
        );

        private static readonly IEnumerable<string> Empty = Enumerable.Empty<string>();
        private IEnumerable<string> Expand(IEnumerable<string> collection) =>
            collection.Select(item => Environment.ExpandEnvironmentVariables(item));
    }
}
