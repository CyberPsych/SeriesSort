using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SeriesSort.Model.Interface;

namespace SeriesSort.Model.Model
{
    public class Episode : IEpisode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public virtual int EpisodeId { get; set; }

        public virtual Series Series { get; set; }
        public virtual int Season { get; set; }
        public virtual int EpisodeNumber { get; set; }
    }
}