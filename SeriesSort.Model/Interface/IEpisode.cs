using SeriesSort.Model.Model;

namespace SeriesSort.Model.Interface
{
    public interface IEpisode
    {
        int EpisodeId { get; set; }
        Series Series { get; set; }
        int Season { get; }
        int EpisodeNumber { get; }
    }
}