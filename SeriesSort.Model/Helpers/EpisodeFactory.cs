namespace SeriesSort.Model.Helpers
{
    public class EpisodeFactory
    {
        private readonly MediaModelDBContext _dbContext;

        public EpisodeFactory(MediaModelDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Episode CreateNewEpisode(string fullPath)
        {
            var episode = new Episode(fullPath);
            var episodeSeriesIntoExtractor = new EpisodeSeriesInformationExtractor(new SeriesQueryByNameFromDbContext(_dbContext));
            episodeSeriesIntoExtractor.ExtractInfo(episode);
            return episode;
        }
    }
}
