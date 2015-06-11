using System.Collections.Generic;
using System.Linq;

namespace SeriesSort.Model.Helpers
{
    public class MediaTypeHelper
    {
        private readonly MediaModelDBContext _dbContext;


        public MediaTypeHelper(MediaModelDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<string> GetValidExtensions()
        {
            var validExtensions = new List<string>();
            var mediaTypes = _dbContext.MediaTypes.Where(mt => mt.ValidForSeries);
            foreach (var mediaType in mediaTypes)
            {
                validExtensions.Add(mediaType.Extension);
            }
            return validExtensions;
        }
    }
}