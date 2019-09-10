using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class AssetPhotosModel
    {
        public Asset asset { get; set; }
        public List<byte[]> photos { get; set; }
        public Item item { get; set; }
        public AssetPhotosModel() {
            photos = new List<byte[]>();
        }
    }
}