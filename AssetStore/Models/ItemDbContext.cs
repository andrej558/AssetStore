using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class ItemDbContext: DbContext
    {
        public DbSet<Item> Assets { get; set; }

        public ItemDbContext(): base("Items"){    
        }
    }
}