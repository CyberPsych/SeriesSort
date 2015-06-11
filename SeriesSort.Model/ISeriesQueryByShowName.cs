namespace SeriesSort.Model
{
    public interface ISeriesQueryByShowName
    {
        Series GetSeriesBySeriesName(string seriesName);
    }
}