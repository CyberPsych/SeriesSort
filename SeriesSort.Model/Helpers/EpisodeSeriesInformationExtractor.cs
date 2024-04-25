using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;
using System;
using System.Text.RegularExpressions;

namespace SeriesSort.Model.Helpers
{


    public class EpisodeSeriesInformationExtractor : IEpisodeSeriesInformationExtractor
    {
        private readonly ISeriesQueryByShowName _seriesQuery;
        private EpisodeFile _episodeFile;
        private DateTime _creationDate;
        private int _episodeNumber;
        private int _season;
        private string _showName;
        private readonly IEpisodeFileNameCleaner _episodeFileNameCleaner;
        private readonly EpisodeFileInformationRetriever _episodeFileInformationRetriever;

        public EpisodeSeriesInformationExtractor(ISeriesQueryByShowName seriesQuery, IEpisodeFileNameCleaner episodeFileNameCleaner, EpisodeFileInformationRetriever episodeFileInformationRetriever)
        {
            _seriesQuery = seriesQuery;
            _episodeFileNameCleaner = episodeFileNameCleaner;
            _episodeFileInformationRetriever = episodeFileInformationRetriever;
        }

        public EpisodeFile ExtractInfo(EpisodeFile episodeFile)
        {
            _episodeFile = episodeFile;

            if (_episodeFile.FileName == null)
                throw new MissingFieldException("Filename is required to extract file info.");
            if (_episodeFile.FullPath == null)
                throw new MissingFieldException("Fullpath is required to extract file info.");

            _creationDate = DateTime.Now;

            const string fov = @"(?<ShowName>.*?)S(?<Season>\d{1,2})E(?<Episode>\d{1,2})";
            var regexStandard = new Regex(fov, RegexOptions.IgnoreCase);
            var match = regexStandard.Match(_episodeFile.FileName);

            if (match.Success)
            {
                foreach (var groupName in regexStandard.GetGroupNames())
                {
                    var group = match.Groups[groupName];
                    switch (groupName)
                    {
                        case "ShowName":
                            _showName = _episodeFileNameCleaner.CleanUpName(@group.Value);
                            break;
                        case "Season":
                            _season = Convert.ToInt32(@group.Value);
                            break;
                        case "Episode":
                            _episodeNumber = Convert.ToInt32(@group.Value);
                            break;
                    }
                }

                if (_showName != null)
                {
                    _episodeFile.Series = _seriesQuery.GetSeriesBySeriesName(_showName);
                }
            }

            _episodeFile = _episodeFileInformationRetriever.GetFileInfoForEpisode(_episodeFile);

            _episodeFile.SetInfoFromFile(_season, _episodeNumber, _creationDate);

            return _episodeFile;
        }
    }
}