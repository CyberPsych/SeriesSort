using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using System.Data.Entity;
using System.Windows;
using System.Windows.Data;
using WPFFolderBrowser;

namespace SeriesSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MediaModelDBContext _dbContext;

        public MainWindow()
        {
            InitializeComponent();

            _dbContext = new MediaModelDBContext();
        }

        private void ClickIndexEpisodesButton(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentPath.Text) )
            {
                var episodesHelper = new EpisodesHelper(_dbContext);
                var episodes = episodesHelper.GetEpisodeFiles(CurrentPath.Text, true);
                foreach (var episode in episodes)
                {
                    _dbContext.EpisodeFiles.Add(episode);
                }
                _dbContext.SaveChanges();
            }
        }

        private void ClickPathButton(object sender, RoutedEventArgs e)
        {
            var folderBrowser = new WPFFolderBrowserDialog();
            var showDialog = folderBrowser.ShowDialog();
            if (showDialog != null && (bool) showDialog)
            {
                CurrentPath.Text = folderBrowser.FileName;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var seriesViewSource = ((CollectionViewSource) (FindResource("SeriesViewSource")));

            _dbContext.Series.Load();
            _dbContext.EpisodeFiles.Load();
            seriesViewSource.Source = _dbContext.Series.Local;
        }

        private void ClickOrganiseEpisodesButton(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentPath.Text))
            {
                var episodesHelper = new EpisodesHelper(_dbContext);
                var episodes = episodesHelper.GetEpisodeFiles(CurrentPath.Text, true);
                foreach (var episode in episodes)
                {
                    var episodeHelper = new EpisodeFileLibraryMover(episode);
                    episodeHelper.MoveToLibrary(Settings.SeriesLibraryPath);
                }
                _dbContext.SaveChanges();
            }
        }
    }
}
