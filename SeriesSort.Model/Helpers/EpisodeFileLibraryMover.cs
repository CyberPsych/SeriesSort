using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SeriesSort.Model.Helpers
{
    public class EpisodeFileLibraryMover : IEpisodeFileLibraryMover
    {
        private readonly EpisodeFile _episodeFile;

        public EpisodeFileLibraryMover(EpisodeFile episodeFile)
        {
            _episodeFile = episodeFile;
        }

        public void MoveToLibrary(string libraryDirectory)
        {
            var libraryFullPath = _episodeFile.CreateLibraryEpisodePath(libraryDirectory);

            var fileInfo = new FileInfo(libraryFullPath);
            var directoryName = fileInfo.DirectoryName;
            if (!string.IsNullOrEmpty(directoryName))
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
                    libraryFullPath = _episodeFile.GetNextEpisodePathForDuplicate(libraryFullPath);
                }
            }

            File.Move(_episodeFile.FullPath, libraryFullPath);
            _episodeFile.FullPath = libraryFullPath;
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
                throw new UnauthorizedAccessException("Could not delete file.", unauthorizedAccessException);
            }
        }
    }
}