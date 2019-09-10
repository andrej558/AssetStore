using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class AssetsDbContext: DbContext
    {
        public DbSet<Asset2D> Assets2D { get; set; }
        public DbSet<Asset3D> Assets3D { get; set; }
        public DbSet<AssetAudio> AssetsAudio { get; set; }
        public DbSet<AssetTools> AssetsTools { get; set; }
        public AssetsDbContext() : base("Assets") {}
        public static AssetsDbContext Create() {
            return new AssetsDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AssetsDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}