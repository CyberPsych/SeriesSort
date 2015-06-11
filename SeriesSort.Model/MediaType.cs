using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeriesSort.Model
{
    public class MediaType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int MediaTypeId { get; set; }
        public string Extension { get; set; }
        public bool ValidForSeries { get; set; }
    }
}