namespace SeriesSort.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Episodes", "FileSize", c => c.Double(nullable: false));
            DropColumn("dbo.Episodes", "ValidEpisode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Episodes", "ValidEpisode", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Episodes", "FileSize", c => c.Long(nullable: false));
        }
    }
}
