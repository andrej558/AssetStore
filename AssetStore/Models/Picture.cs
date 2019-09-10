using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        
    }
}