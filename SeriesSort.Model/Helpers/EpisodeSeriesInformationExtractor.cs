using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;

namespace SeriesSort.Model.Helpers
{


    public class EpisodeSeriesInformationExtractor : IEpisodeSeriesInformationExtractor
    {
        private readonly ISeriesQueryByShowName _seriesQuery;
        private EpisodeFile _episodeFile;
        private DateTime _creationDate;
        private double _fileSize;
        private int _episodeNumber;
        private int _season;
        private string _showName;

        public EpisodeSeriesInformationExtractor(ISeriesQueryByShowName seriesQuery)
        {
            _seriesQuery = seriesQuery;
        }

        public void ExtractInfo(EpisodeFile episodeFile)
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
                    _episodeFile.Series = _seriesQuery.GetSeriesBySeriesName(_showName);
                }
            }

            GetFileInfoForEpisode(_episodeFile);

            _episodeFile.SetInfoFromFile(_season, _episodeNumber, _fileSize, _creationDate);
        }

        private void GetFileInfoForEpisode(EpisodeFile episodeFile)
        {
            if (File.Exists(episodeFile.FullPath))
            {
                _creationDate = File.GetCreationTime(episodeFile.FullPath);
                var fileInfo = new FileInfo(episodeFile.FullPath);
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