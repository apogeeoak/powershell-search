namespace Apogee.Search.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Management.Automation;

    /// <summary> Find modified file system items. </summary>
    [Cmdlet(VerbsCommon.Find, "Modified")]
    [OutputType(typeof(string))]
    public class FindModified : PSCmdlet
    {
        private List<string> paths = new List<string>();
        private Settings settings = new Settings();

        /// <summary> Path to search. </summary>
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string[] Path { get; set; } = new[] { string.Empty };

        /// <summary> Recursion switch. Defaults to false which only shows current contents. </summary>
        [Parameter]
        public SwitchParameter Recurse { get; set; }

        /// <summary> Recursion depth, automatically sets Recurse. Defaults to uint.MaxValue which performs a full recursion. </summary>
        [Parameter]
        public uint Depth { get => depth; set { depth = value; Recurse = true; } }
        private uint depth = uint.MaxValue;

        /// <summary> DateTime after which to search for modified items. Defaults to zero which includes everything. </summary>
        [Parameter]
        public DateTime After { get; set; }

        /// <summary> Exclusion level for files and folders. Uses the value set in the settings file if not provided. If neither are set defaults to [ExcludeLevel]::None which excludes nothing. </summary>
        [Parameter]
        public ExcludeLevel? ExcludeLevel { get; set; }

        /// <summary> Display progress switch. Defaults to false which does not display incremental progress. </summary>
        [Parameter]
        public SwitchParameter DisplayProgress { get; set; }

        protected override void ProcessRecord()
        {
            if (Path == null || Path.Length == 0) Path = new[] { string.Empty };

            foreach (var path in Path)
                foreach (var resolvedPath in GetResolvedProviderPathFromPSPath(path, out ProviderInfo _))
                    paths.Add(resolvedPath);

            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            var progressRecord = (DisplayProgress) ? new ProgressRecord(0, "Find-Modified", "Searching...") : null;
            settings.Read(ExcludeLevel);

            foreach (var path in paths)
                foreach (var item in GetModified(path, Recurse, Depth, After, settings, progressRecord))
                    WriteObject(item);

            base.EndProcessing();
        }

        private IEnumerable<string> GetModified(string path, bool recurse, uint depth, DateTime after, Settings settings, ProgressRecord? progressRecord)
        {
            var discovered = new Stack<(string path, uint depth)>();
            discovered.Push((path, depth));

            while (discovered.Count > 0)
            {
                (path, depth) = discovered.Pop();

                var (files, directories) = TryGetFileSystemEntries(path);

                // Write progress if not null.
                WriteProgress(progressRecord, path);

                // Return directories.
                foreach (var directory in directories)
                    if (Modified(directory, after))
                        yield return directory;

                // Return files.
                foreach (var file in files)
                    if (Modified(file, after))
                        if (!settings.ExcludeFile(file))
                            yield return file;

                // Add directories to discovered.
                if (recurse && depth > 0)
                    for (int i = directories.Length - 1; i >= 0; --i)
                        if (!settings.ExcludeFolder(directories[i]))
                            discovered.Push((directories[i], depth - 1));
            }
        }

        private (string[] Files, string[] Directories) TryGetFileSystemEntries(string path)
        {
            try
            {
                return (Directory.GetFiles(path), Directory.GetDirectories(path));
            }
            catch (UnauthorizedAccessException ex)
            {
                WriteError(new ErrorRecord(ex, "UnauthorizedAccess", ErrorCategory.PermissionDenied, path));
                return (Array.Empty<string>(), Array.Empty<string>());
            }
            catch (DirectoryNotFoundException ex)
            {
                WriteError(new ErrorRecord(ex, "DirectoryNotFound", ErrorCategory.ObjectNotFound, path));
                return (Array.Empty<string>(), Array.Empty<string>());
            }
        }

        private void WriteProgress(ProgressRecord? progressRecord, string path)
        {
            if (progressRecord is null)
                return;

            progressRecord.StatusDescription = $"Searching {path}...";
            WriteProgress(progressRecord);
        }

        private bool Modified(string item, DateTime after) => Directory.GetLastWriteTime(item) > after;
    }
}
