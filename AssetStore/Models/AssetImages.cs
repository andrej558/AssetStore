using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class AssetImages
    {
        public Asset asset { get; set; }
        [Required(ErrorMessage = "Upload at least 1 image")]
        [Display(Name = "Upload screenshots of asset (the first image will serve as thumbnail also).")]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        [Required(ErrorMessage = "Upload the asset correctly.")]
        [Display(Name = "Upload Asset (IT MUST BE IN ZIP/RAR FORMAT!!!).")]
        public HttpPostedFileBase File { get; set; }
    }
}