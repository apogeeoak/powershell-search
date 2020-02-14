namespace Apogee.Search.Model
{
    using System;

    public class Excluded
    {
        private string[][] excluded = new string[0][];

        public string[] Minimal { get; set; } = Array.Empty<string>();
        public string[] Standard { get; set; } = Array.Empty<string>();
        public string[] Additional { get; set; } = Array.Empty<string>();

        public bool Exclude(string item)
        {
            foreach (var array in excluded)
                if (Exclude(array, item)) return true;
            return false;
        }

        public void PostProcess(ExcludeLevel level)
        {
            Initialize(out excluded, level);
            foreach (var array in excluded)
                Expand(array);
        }

        private bool Exclude(string[] array, string item) =>
            (Array.Exists(array, excluded => item.StartsWith(excluded, StringComparison.OrdinalIgnoreCase)));

        private void Expand(string[] array)
        {
            for (int i = 0; i < array.Length; ++i)
                array[i] = Environment.ExpandEnvironmentVariables(array[i]);
        }

        private void Initialize(out string[][] excluded, ExcludeLevel level) => excluded = level switch
        {
            ExcludeLevel.None => new string[0][],
            ExcludeLevel.Minimal => new[] { Minimal },
            ExcludeLevel.Standard => new[] { Minimal, Standard },
            ExcludeLevel.Additional => new[] { Minimal, Standard, Additional },
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, "Parameter is not a valid enumeration value.")
        };
    }
}
