using System.Linq;
using SeriesSort.Model.Interface;

namespace SeriesSort.Model.Helpers
{
    public class EpisodeFileNameCleaner : IEpisodeFileNameCleaner
    {
        public string CleanUpName(string name)
        {
            var trimChars = new[] { '_', '.', ' ' };
            var workingName = name.Trim(trimChars);

            return trimChars.Aggregate(workingName, (current, trimChar) => current.Replace(trimChar, ' '));
        }
    }
}