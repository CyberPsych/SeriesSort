using SeriesSort.Model.Model;

namespace SeriesSort.Model.Interface
{
    public interface ISeriesQueryByShowName
    {
        Series GetSeriesBySeriesName(string seriesName);
    }
}