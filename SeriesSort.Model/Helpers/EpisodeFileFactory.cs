using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;

namespace SeriesSort.Model.Helpers
{
    public class EpisodeFileFactory
    {
        private EpisodeFile _episodeFile;
        private readonly IEpisodeSeriesInformationExtractor _episodeSeriesInformationExtractor;

        public EpisodeFileFactory(IEpisodeSeriesInformationExtractor episodeSeriesInformationExtractor)
        {
            _episodeSeriesInformationExtractor = episodeSeriesInformationExtractor;
        }

        public EpisodeFile CreateNewEpisode(string fullPath)
        {
            _episodeFile = new EpisodeFile(fullPath);
            _episodeSeriesInformationExtractor.ExtractInfo(_episodeFile);
            return _episodeFile;
        }
    }
}
