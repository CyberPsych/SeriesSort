using SeriesSort.Model.Model;

namespace SeriesSort.Model.Interface
{
    public interface IEpisodeSeriesInformationExtractor
    {
        EpisodeFile ExtractInfo(EpisodeFile episodeFile);
    }
}