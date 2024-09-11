using DTKH2024.SbinSolution.CategoryPromotions;
using DTKH2024.SbinSolution.ProductPromotions;
using DTKH2024.SbinSolution.Products;
using DTKH2024.SbinSolution.ProductTypes;
using DTKH2024.SbinSolution.HistoryTypes;
using DTKH2024.SbinSolution.TransactionStatuses;
using DTKH2024.SbinSolution.BenefitsRankLevels;
using DTKH2024.SbinSolution.RankLevels;
using DTKH2024.SbinSolution.Devices;
using DTKH2024.SbinSolution.StatusDevices;
using DTKH2024.SbinSolution.Brands;
using System.Collections.Generic;
using System.Text.Json;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DTKH2024.SbinSolution.Authorization.Delegation;
using DTKH2024.SbinSolution.Authorization.Roles;
using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.Chat;
using DTKH2024.SbinSolution.Editions;
using DTKH2024.SbinSolution.ExtraProperties;
using DTKH2024.SbinSolution.Friendships;
using DTKH2024.SbinSolution.MultiTenancy;
using DTKH2024.SbinSolution.MultiTenancy.Accounting;
using DTKH2024.SbinSolution.MultiTenancy.Payments;
using DTKH2024.SbinSolution.OpenIddict.Applications;
using DTKH2024.SbinSolution.OpenIddict.Authorizations;
using DTKH2024.SbinSolution.OpenIddict.Scopes;
using DTKH2024.SbinSolution.OpenIddict.Tokens;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.EntityFrameworkCore
{
    public class SbinSolutionDbContext : AbpZeroDbContext<Tenant, Role, User, SbinSolutionDbContext>, IOpenIddictDbContext
    {
        public virtual DbSet<CategoryPromotion> CategoryPromotions { get; set; }

        public virtual DbSet<ProductPromotion> ProductPromotions { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductType> ProductTypes { get; set; }

        public virtual DbSet<HistoryType> HistoryTypes { get; set; }

        public virtual DbSet<TransactionStatus> TransactionStatuses { get; set; }

        public virtual DbSet<BenefitsRankLevel> BenefitsRankLevels { get; set; }

        public virtual DbSet<RankLevel> RankLevels { get; set; }

        public virtual DbSet<Device> Devices { get; set; }

        public virtual DbSet<StatusDevice> StatusDevices { get; set; }

        public virtual DbSet<Brand> Brands { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<OpenIddictApplication> Applications { get; }

        public virtual DbSet<OpenIddictAuthorization> Authorizations { get; }

        public virtual DbSet<OpenIddictScope> Scopes { get; }

        public virtual DbSet<OpenIddictToken> Tokens { get; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<SubscriptionPaymentProduct> SubscriptionPaymentProducts { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public SbinSolutionDbContext(DbContextOptions<SbinSolutionDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new { e.TenantId }); });

            modelBuilder.Entity<SubscriptionPayment>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<SubscriptionPaymentProduct>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigureOpenIddict();
        }
    }
}