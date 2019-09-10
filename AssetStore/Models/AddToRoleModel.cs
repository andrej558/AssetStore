using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetStore.Models
{
    public class AddToRoleModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public List<string> roles { get; set; }

        public AddToRoleModel() {
            roles = new List<string>();
        }
    }
}