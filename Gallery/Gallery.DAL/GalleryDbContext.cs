using Gallery.DAL.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gallery.DAL
{
    public class GalleryDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();

        public DbSet<Picture> Pictures => Set<Picture>();

        public DbSet<Album> Albums => Set<Album>();


        public GalleryDbContext(DbContextOptions<GalleryDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.FavoritedAlbums)
                .WithMany(e => e.FavoritedBy)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteAlbums",
                    j => j
                        .HasOne<Album>()
                        .WithMany(),
                    j => j
                        .HasOne<ApplicationUser>()
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction)
                );

            builder.Entity<Album>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.CreatedAlbums);
        }
    }
}
