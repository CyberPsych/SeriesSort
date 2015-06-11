using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeriesSort.Model.Helpers
{
    public class EpisodeHelper
    {
        private readonly Episode _episode;

        public EpisodeHelper(Episode episode)
        {
            _episode = episode;
        }

        public void MoveToLibrary(string libraryDirectory)
        {
            var libraryFullPath = _episode.CreateLibraryEpisodePath(libraryDirectory);

            var fileInfo = new FileInfo(libraryFullPath);
            var directoryName = fileInfo.DirectoryName;
            if (!String.IsNullOrEmpty(directoryName))
            {
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
            }
            else
            {
                throw new Exception("Directory does not exist.");
            }

            if (File.Exists(libraryFullPath))
            {
                if (Settings.OverwriteFiles)
                {
                    File.Delete(libraryFullPath);
                }
                else
                {
                    libraryFullPath = _episode.GetNextEpisodePathForDuplicate(libraryFullPath);
                }
            }

            File.Move(_episode.FullPath, libraryFullPath);
            _episode.FullPath = libraryFullPath;
        }
    }
}