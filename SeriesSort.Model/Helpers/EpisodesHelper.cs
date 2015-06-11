using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeriesSort.Model.Helpers
{
    public class EpisodesHelper
    {
        private readonly MediaModelDBContext _dbContext;

        public EpisodesHelper(MediaModelDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Episode> GetEpisodeFiles(string path, bool searchSubdirectories)
        {
            var files = Directory.GetFiles(path, "*.*", searchSubdirectories? SearchOption.AllDirectories: SearchOption.TopDirectoryOnly);
            var episodeFactory = new EpisodeFactory(_dbContext);

            return files.Select(file => episodeFactory.CreateNewEpisode((file))).Where(episode => episode.IsValidEpisode()).ToList();
        }
    }
}