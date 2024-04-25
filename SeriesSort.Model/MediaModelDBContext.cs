using SeriesSort.Model.Model;
using System.Data.Entity;

namespace SeriesSort.Model
{
    public class MediaModelDBContext : DbContext
    {
        // Your context has been configured to use a 'MediaModelDBContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SeriesSort.Model.MediaModelDBContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MediaModelDBContext' 
        // connection string in the application configuration file.
        public MediaModelDBContext()
            : base("name=MediaModelDBContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<Series> Series { get; set; }
        public DbSet<EpisodeFile> EpisodeFiles { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
    }
}