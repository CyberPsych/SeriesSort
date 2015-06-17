using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

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
                    AdminDelete(libraryFullPath);
                }
                else
                {
                    libraryFullPath = _episode.GetNextEpisodePathForDuplicate(libraryFullPath);
                }
            }

            _episode.FullPath = libraryFullPath;
        }

        private void AdminDelete(string libraryFullPath)
        {
            var dInfo = new DirectoryInfo(libraryFullPath);
            var dSecurity = dInfo.GetAccessControl();

            dSecurity.AddAccessRule(
                new FileSystemAccessRule(
                    new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), 
                    FileSystemRights.DeleteSubdirectoriesAndFiles, 
                    AccessControlType.Allow)
                    );
            dInfo.SetAccessControl(dSecurity);

            try
            {
                File.Delete(libraryFullPath);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                //TODO: Handle Exceptions
            }
        }
    }
}