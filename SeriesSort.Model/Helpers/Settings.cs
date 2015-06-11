namespace SeriesSort.Model.Helpers
{
    public static class Settings
    {
        static Settings()
        {
            ExcludeSampleFiles = true;
            SeriesLibraryPath = @"C:\UnitTestTemp\SeriesSort\";
            OverwriteFiles = true;
        }
        public static bool ExcludeSampleFiles { get; set; }
        public static string SeriesLibraryPath { get; set; }
        public static bool OverwriteFiles { get; set; }
    }
}