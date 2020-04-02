namespace Apogee.Search.Model
{
    public class CommandParameters
    {
        public CommandParameters(ExcludeLevel? level)
        {
            ExcludeLevel = level;
        }

        public ExcludeLevel? ExcludeLevel { get; set; }
    }
}
