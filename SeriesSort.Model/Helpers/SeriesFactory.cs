using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;

namespace SeriesSort.Model.Helpers
{
    public class SeriesFactory
    {
        private readonly ISeriesQueryByShowName _seriesQuery;

        public SeriesFactory(ISeriesQueryByShowName seriesQuery)
        {
            _seriesQuery = seriesQuery;
        }

        public Series CreateNewSeries(string seriesName)
        {
            var series = _seriesQuery.GetSeriesBySeriesName(seriesName) ?? new Series() {SeriesName = seriesName};
            return series;
        }
    }
}
