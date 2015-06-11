namespace SeriesSort.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nextA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "FileExtention", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Episodes", "FileExtention");
        }
    }
}
