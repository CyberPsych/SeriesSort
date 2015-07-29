using SeriesSort.Model.Model;

namespace SeriesSort.Model.Interface
{
    public interface IEpisodeFileInformationRetriever
    {
        EpisodeFile GetFileInfoForEpisode(EpisodeFile episodeFile);
    }
}