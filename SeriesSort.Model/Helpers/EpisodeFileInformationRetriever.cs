using System.IO;
using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;

namespace SeriesSort.Model.Helpers
{
    public class EpisodeFileInformationRetriever : IEpisodeFileInformationRetriever
    {
        public EpisodeFile GetFileInfoForEpisode(EpisodeFile episodeFile)
        {
            var newEpisodeFile = episodeFile;
            if (File.Exists(episodeFile.FullPath))
            {
                newEpisodeFile.CreateDateTime = File.GetCreationTime(episodeFile.FullPath);
                var fileInfo = new FileInfo(episodeFile.FullPath);
                newEpisodeFile.FileSize = fileInfo.Length;
            }
            return newEpisodeFile;
        }
    }
}