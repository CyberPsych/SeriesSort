using System.Collections.Generic;
using System.IO;
using System.Linq;
using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;

namespace SeriesSort.Model.Helpers
{
    public class EpisodesHelper
    {
        private readonly MediaModelDBContext _dbContext;

        public EpisodesHelper(MediaModelDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<EpisodeFile> GetEpisodeFiles(string path, bool searchSubdirectories)
        {
            var files = Directory.GetFiles(path, "*.*", searchSubdirectories? SearchOption.AllDirectories: SearchOption.TopDirectoryOnly);
            ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(_dbContext);
            var episodeFactory = new EpisodeFileFactory(new EpisodeSeriesInformationExtractor(seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever()));

            return files.Select(file => episodeFactory.CreateNewEpisode((file))).Where(episode => episode.IsValidEpisode()).ToList();
        }
    }
}