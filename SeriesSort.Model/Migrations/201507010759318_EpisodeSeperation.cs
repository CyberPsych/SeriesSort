namespace SeriesSort.Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class EpisodeSeperation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EpisodeFiles",
                c => new
                {
                    EpisodeId = c.Int(nullable: false, identity: true),
                    FileName = c.String(),
                    FullPath = c.String(),
                    CreateDateTime = c.DateTime(nullable: false),
                    FileSize = c.Double(nullable: false),
                    FileExtention = c.String(),
                    Season = c.Int(nullable: false),
                    EpisodeNumber = c.Int(nullable: false),
                    Series_SeriesId = c.Int(),
                })
                .PrimaryKey(t => t.EpisodeId)
                .ForeignKey("dbo.Series", t => t.Series_SeriesId)
                .Index(t => t.Series_SeriesId);

            CreateTable(
                "dbo.Series",
                c => new
                {
                    SeriesId = c.Int(nullable: false, identity: true),
                    SeriesName = c.String(),
                })
                .PrimaryKey(t => t.SeriesId);

            CreateTable(
                "dbo.MediaTypes",
                c => new
                {
                    MediaTypeId = c.Int(nullable: false, identity: true),
                    Extension = c.String(),
                    ValidForSeries = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.MediaTypeId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EpisodeFiles", "Series_SeriesId", "dbo.Series");
            DropIndex("dbo.EpisodeFiles", new[] { "Series_SeriesId" });
            DropTable("dbo.MediaTypes");
            DropTable("dbo.Series");
            DropTable("dbo.EpisodeFiles");
        }
    }
}
