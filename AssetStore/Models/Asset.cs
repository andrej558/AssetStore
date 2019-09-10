using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace AssetStore.Models
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }  //key
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "The description field is required")]
        [Display(Name = "Describe the asset briefly")]
        public string Description { get; set; }
        public string Publisher { get; set; }
        [Required(ErrorMessage = "Select platform")]
        public string Platform { get; set; }
        [Required(ErrorMessage = "The Price field is required")]
        [Display(Name = "Enter price in €")]
        public float Price { get; set; }
        public float Size { get; set; }  //in MB
        public string ReleaseDate { get; set; }
        [Required(ErrorMessage = "Select type of asset")]
        [Display(Name = "Type of Asset")]
        public string Type { get; set; }   //the type of asset
        public string imagesId { get; set; }
        public byte[] assetZip { get; set; }
    }
}