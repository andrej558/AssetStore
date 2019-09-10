using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class PictureDbContext: DbContext
    {
        public DbSet<Picture> AssetsPhotos { get; set; }

        public PictureDbContext() : base("Photos") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<PictureDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}