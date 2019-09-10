using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AssetStore.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<AssetStore.Models.Asset> Assets { get; set; }

        public System.Data.Entity.DbSet<AssetStore.Models.Asset2D> Asset2D { get; set; }

        public System.Data.Entity.DbSet<AssetStore.Models.Asset3D> Asset3D { get; set; }

        public System.Data.Entity.DbSet<AssetStore.Models.AssetTools> AssetTools { get; set; }

        public System.Data.Entity.DbSet<AssetStore.Models.AssetAudio> AssetAudios { get; set; }

        public System.Data.Entity.DbSet<AssetStore.Models.Picture> Pictures { get; set; }

        public System.Data.Entity.DbSet<AssetStore.Models.Item> Items { get; set; }

        public System.Data.Entity.DbSet<AssetStore.Models.PublisherModel> PublisherModels { get; set; }
    }
}