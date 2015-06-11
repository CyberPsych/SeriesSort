using System.Collections.Generic;
using System.IO;

namespace SeriesSort.Tests.Lib
{
    internal static class HelperFileFolderTests
    {
        internal static void CreateTestFolder(string testDir)
        {
            if (!Directory.Exists(testDir))
            {
                Directory.CreateDirectory(testDir);
            }
        }

        internal static void DisposeTestFolder(string testDir)
        {
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
        }

        internal static void AddTestFileToTestFolder(string testDir, IEnumerable<string> filesToAdd, string testName)
        {
            foreach (var filename in filesToAdd)
            {
                var path = string.Format(@"{0}\{1}", testDir, filename);
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                    File.AppendAllLines(path, new[] { string.Format("File Created for unit test {0}", testName) });
                }
            }
        }
    }
}
