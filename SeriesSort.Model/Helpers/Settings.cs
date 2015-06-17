using System.Configuration;

namespace SeriesSort.Model.Helpers
{
    public static class Settings
    {
        static Settings()
        {
            ExcludeSampleFiles = true;
            SeriesLibraryPath =  ConfigurationManager.AppSettings["SeriesPath"];
            OverwriteFiles = true;
        }
        public static bool ExcludeSampleFiles { get; set; }
        public static string SeriesLibraryPath { get; set; }
        public static bool OverwriteFiles { get; set; }
    }
}