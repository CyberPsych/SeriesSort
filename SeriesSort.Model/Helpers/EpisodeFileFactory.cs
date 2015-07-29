using System.Diagnostics;
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
            return _episodeSeriesInformationExtractor.ExtractInfo(new EpisodeFile(fullPath));
        }
    }
}
