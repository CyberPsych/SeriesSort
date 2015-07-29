using System;
using System.IO;
using SeriesSort.Model.Helpers;

namespace SeriesSort.Model.Model
{
    public class EpisodeFile : Episode
    {
        public virtual string FileName { get; protected set; }
        public virtual string FullPath
        {
            get
            {
                return _fullPath;
            }
            set
            {
                _fullPath = value;
                FileName = GetFileName(_fullPath);
                FileExtention = GetFileExtension(_fullPath);
            }
        }

        private string GetFileExtension(string fullPath)
        {
            var lastDecimalPoint = fullPath.LastIndexOf('.') + 1;   
            return fullPath.Substring(lastDecimalPoint, fullPath.Length - lastDecimalPoint);
        }

        public virtual DateTime CreateDateTime { get; set; }
        public virtual double FileSize { get; set; }
        public virtual string FileExtention { get; set; } 

        private string _fullPath;

        public EpisodeFile()
        {
            CreateDateTime = DateTime.Now;
        }

        public EpisodeFile(string fullPath)
        {
            FullPath = fullPath;
            CreateDateTime = DateTime.Now;
        }

        public bool IsValidEpisode()
        {
            var isValidEpisode = Season != 0 && Series.SeriesName != null && EpisodeNumber != 0;

            var mediaTypeHelper = new MediaTypeHelper(new MediaModelDBContext());
            var listValidExtensions = mediaTypeHelper.GetValidExtensions();
            if (!listValidExtensions.Contains(FileExtention))
            {
                isValidEpisode = false;
            }
            if (Settings.ExcludeSampleFiles)
            {
                if (Series != null && Series.SeriesName != null && Series.SeriesName.ToUpper().Contains("SAMPLE"))
                {
                    isValidEpisode = false;
                }
            }
            return isValidEpisode;
        }

        public override string ToString()
        {
            return string.Format("{0} S{1:00}E{2:00}.{3}", Series.SeriesName, Season, EpisodeNumber, FileExtention);
        }

        public string CreateLibraryEpisodePath(string libraryPath)
        {
            return string.Format("{0}\\{1}\\Season {2:00}\\{3}", libraryPath, Series.SeriesName, Season, ToString());
        }

        public string GetNextEpisodePathForDuplicate(string libraryPath)
        {
            var copyNumber = 1;
            var episodePathForDuplicate = GetEpisodePathForDuplicate(libraryPath, copyNumber);
            while (File.Exists(episodePathForDuplicate))
            {
                copyNumber++;
                episodePathForDuplicate = GetEpisodePathForDuplicate(libraryPath, copyNumber);
            }
            return episodePathForDuplicate;
        }

        public string GetEpisodePathForDuplicate(string libraryPath, int copyNumber)
        {
            var episodePathForDuplicate = string.Format("{0}\\{1}\\Season {2:00}\\{1} S{2:00}E{3:00} Copy{5:00}.{4}", libraryPath, Series.SeriesName, Season, EpisodeNumber, FileExtention, copyNumber);


            return episodePathForDuplicate;
        }

        public void SetInfoFromFile(int season, int episodeNumber, DateTime creationdate)
        {
            Season = season;
            EpisodeNumber = episodeNumber;
            CreateDateTime = creationdate;
        }

        private string GetFileName(string name)
        {
            var fileInfo = new FileInfo(name);
            var fileName = fileInfo.Name;
            return fileName;
        }
    }
}