namespace AssetStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Asset2D", "Rating");
            DropColumn("dbo.Asset3D", "Rating");
            DropColumn("dbo.AssetAudios", "Rating");
            DropColumn("dbo.AssetTools", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetTools", "Rating", c => c.Single(nullable: false));
            AddColumn("dbo.AssetAudios", "Rating", c => c.Single(nullable: false));
            AddColumn("dbo.Asset3D", "Rating", c => c.Single(nullable: false));
            AddColumn("dbo.Asset2D", "Rating", c => c.Single(nullable: false));
        }
    }
}
