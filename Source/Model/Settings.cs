namespace Apogee.Search.Model
{
    public class Settings
    {
        public ExcludeLevel ExcludeLevel = ExcludeLevel.None;
        public Excluded ExcludedFolders = new Excluded();
        public Excluded ExcludedFiles = new Excluded();

        public bool ExcludeFolder(string item) => ExcludedFolders.Exclude(item);
        public bool ExcludeFile(string item) => ExcludedFiles.Exclude(item);

        public void PostProcess(ExcludeLevel? parameterLevel)
        {
            var level = parameterLevel ?? ExcludeLevel;
            ExcludedFolders.PostProcess(level);
            ExcludedFiles.PostProcess(level);
        }
    }
}
