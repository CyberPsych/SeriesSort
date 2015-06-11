namespace SeriesSort.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MediaType : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.MediaTypes");
        }
    }
}
