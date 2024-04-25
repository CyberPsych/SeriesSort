using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;
using System.Linq;

namespace SeriesSort.Model
{
    public class SeriesQueryByNameFromDbContext : ISeriesQueryByShowName
    {
        private readonly MediaModelDBContext _dbContext;

        public SeriesQueryByNameFromDbContext(MediaModelDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Series GetSeriesBySeriesName(string seriesName)
        {
            if (_dbContext.Series.Any())
            {
                var matchingSeries = _dbContext.Series.FirstOrDefault(series => series.SeriesName == seriesName);
                if (matchingSeries != null) return matchingSeries;
            }

            var seriesByShowName = new Series() { SeriesName = seriesName };
            _dbContext.Series.Add(seriesByShowName);
            _dbContext.SaveChanges();
            return seriesByShowName;
        }
    }
}