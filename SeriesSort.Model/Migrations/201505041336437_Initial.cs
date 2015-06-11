namespace SeriesSort.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Episodes",
                c => new
                    {
                        EpisodeId = c.Int(nullable: false, identity: true),
                        Season = c.Int(nullable: false),
                        EpisodeNumber = c.Int(nullable: false),
                        FileName = c.String(),
                        FullPath = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        FileSize = c.Long(nullable: false),
                        ValidEpisode = c.Boolean(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Episodes", "Series_SeriesId", "dbo.Series");
            DropIndex("dbo.Episodes", new[] { "Series_SeriesId" });
            DropTable("dbo.Series");
            DropTable("dbo.Episodes");
        }
    }
}
