using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SeriesSort.Model.Helpers;

namespace SeriesSort.Model.Model
{
    public class Series
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public ObservableCollection<EpisodeFile> EpisodeFiles { get; set; }

        public string SeriesPath
        {
            get
            {
                return string.Format("{1}\\{0}", SeriesName, Settings.SeriesLibraryPath);
            }
        }

    }
}