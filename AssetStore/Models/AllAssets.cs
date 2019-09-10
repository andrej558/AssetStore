using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class AllAssets
    {
        public AllAssets()
        {
            assets2D = new List<Asset2D>();
            assets3D = new List<Asset3D>();
            assetsAudio = new List<AssetAudio>();
            assetsTools = new List<AssetTools>() ;
            asset2dImages = new List<byte[]>();
            asset3dImages = new List<byte[]>();
            assetAudioImages = new List<byte[]>();
            assetsToolsImages = new List<byte[]>();
        }

        public Asset asset = new Asset(); 
        public List<Asset2D> assets2D { get; set; }
        public List<byte[]> asset2dImages { get; set; }
        public List<Asset3D> assets3D { get; set; }
        public List<byte[]> asset3dImages { get; set; }
        public List<AssetAudio> assetsAudio { get; set; }
        public List<byte[]> assetAudioImages { get; set; }
        public List<AssetTools> assetsTools { get; set; }
        public List<byte[]> assetsToolsImages { get; set; }
    }

}