﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class Asset2D: Asset
    {
        public Asset2D() { }

        public Asset2D(Asset asset) {
            //copy constructor
            this.Id = asset.Id;
            this.Name = asset.Name;
            this.Platform = asset.Platform;
            this.Price = asset.Price;
            this.Description = asset.Description;
            this.Publisher = asset.Publisher;
            this.Type = asset.Type;
            this.Size = asset.Size;
            this.ReleaseDate = asset.ReleaseDate;
            
            this.imagesId = asset.imagesId;
            this.assetZip = asset.assetZip;
        }
    }
}