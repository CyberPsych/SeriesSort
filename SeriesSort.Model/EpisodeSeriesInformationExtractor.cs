using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeriesSort.Model
{
    public class EpisodeSeriesInformationExtractor : IEpisodeSeriesInformationExtractor
    {
        private readonly ISeriesQueryByShowName _seriesQuery;
        private Episode _episode;
        private DateTime _creationDate;
        private double _fileSize;
        private Series _series;
        private int _episodeNumber;
        private int _season;
        private string _showName;

        public EpisodeSeriesInformationExtractor(ISeriesQueryByShowName seriesQuery)
        {
            _seriesQuery = seriesQuery;
        }

        public void ExtractInfo(Episode episode)
        {
            _episode = episode;

            if (_episode.FileName == null)
                throw new MissingFieldException("Filename is required to extract file info.");
            if (_episode.FullPath == null)
                throw new MissingFieldException("Fullpath is required to extract file info.");

            _creationDate = DateTime.Now;

            const string fov = @"(?<ShowName>.*?)S(?<Season>\d{1,2})E(?<Episode>\d{1,2})";
            var regexStandard = new Regex(fov, RegexOptions.IgnoreCase);
            var match = regexStandard.Match(_episode.FileName);

            if (match.Success)
            {
                foreach (var groupName in regexStandard.GetGroupNames())
                {
                    var group = match.Groups[groupName];
                    switch (groupName)
                    {
                        case "ShowName":
                            _showName = cleanUpName(@group.Value);
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
                    _episode.Series = _seriesQuery.GetSeriesBySeriesName(_showName);
                }
            }

            GetFileInfoForEpisode(_episode);

            _episode.SetInfoFromFile(_season, _episodeNumber, _fileSize, _creationDate);
        }

        private void GetFileInfoForEpisode(Episode episode)
        {
            if (File.Exists(episode.FullPath))
            {
                _creationDate = File.GetCreationTime(episode.FullPath);
                var fileInfo = new FileInfo(episode.FullPath);
                _fileSize = fileInfo.Length;
            }
        }

        private string cleanUpName(string name)
        {
            var trimChars = new[] { '_', '.', ' ' };
            var workingName = name.Trim(trimChars);

            return trimChars.Aggregate(workingName, (current, trimChar) => current.Replace(trimChar, ' '));
        }
    }
}