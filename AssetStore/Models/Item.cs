using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Asset Name")]
        public string name { get; set; }
        public int assetId { get; set; }
        public string assetType { get; set; }
        //public int quantity { get; set; }
        public string buyerId { get; set; }
        public bool isBought { get; set; }
        public bool isInCart { get; set; }
        public byte[] image { get; set; }
        [Display(Name = "Price")]
        public float price { get; set; }
    }
}