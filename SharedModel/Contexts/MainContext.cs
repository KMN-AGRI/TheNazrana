using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedModel.Servers;

namespace SharedModel.Contexts
{
    public class MainContext : IdentityDbContext<Appuser>
    {
        public MainContext(DbContextOptions<MainContext> o) : base(o)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Appuser> AppUsers { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Keypair> Keypairs { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Mediafile> Mediafiles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Orderitem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        public double Distance(float l1, float l2, float l3, float l4)
            => throw new NotSupportedException();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            builder.Entity<Keypair>()
           .Property(application => application.Data)
           .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<object>(v));



            builder.Entity<IdentityRole>(x => x.ToTable("Roles"));
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });

            builder.Entity<Appuser>(x =>
            {
                x.ToTable("AppUsers");

            });

          
        }

    }
}

