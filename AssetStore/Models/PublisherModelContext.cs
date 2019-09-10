using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class PublisherModelContext : DbContext
    {
        public DbSet<PublisherModel> Publishers { get; set; }

        public PublisherModelContext() : base("Publishers") { }
    }
}