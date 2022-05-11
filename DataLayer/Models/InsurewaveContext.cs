using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataLayer.Models
{
    public partial class InsurewaveContext : DbContext
    {
        public InsurewaveContext()
        {
        }

        public InsurewaveContext(DbContextOptions<InsurewaveContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BrokerDetail> BrokerDetails { get; set; } = null!;
        public virtual DbSet<BrokerRequest> BrokerRequests { get; set; } = null!;
        public virtual DbSet<BuyerAsset> BuyerAssets { get; set; } = null!;
        public virtual DbSet<CurrencyConversion> CurrencyConversions { get; set; } = null!;
        public virtual DbSet<InsurerDetail> InsurerDetails { get; set; } = null!;
        public virtual DbSet<PaymentBuyer> PaymentBuyers { get; set; } = null!;
        public virtual DbSet<PolicyDetail> PolicyDetails { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ECSTASY;Database=Insurewave;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrokerDetail>(entity =>
            {
                entity.HasKey(e => e.BrokerId)
                    .HasName("PKBrokerDetails");

                entity.ToTable("Broker.Details");

                entity.Property(e => e.BrokerId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Commission).HasDefaultValueSql("((0.0))");

                entity.Property(e => e.CustomerCount).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Broker)
                    .WithOne(p => p.BrokerDetail)
                    .HasForeignKey<BrokerDetail>(d => d.BrokerId)
                    .HasConstraintName("FK__Broker.De__Broke__32E0915F");
            });

            modelBuilder.Entity<BrokerRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PKBrokerRequests");

                entity.Property(e => e.BrokerId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewStatus)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.BrokerRequests)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("FKBrokerRequestsAssetId");

                entity.HasOne(d => d.Broker)
                    .WithMany(p => p.BrokerRequests)
                    .HasForeignKey(d => d.BrokerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKBrokerRequestsBrokerId");
            });

            modelBuilder.Entity<BuyerAsset>(entity =>
            {
                entity.HasKey(e => e.AssetId)
                    .HasName("PKBuyerAssets");

                entity.ToTable("Buyer.Assets");

                entity.Property(e => e.AssetName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId).HasDefaultValueSql("((1))");

                entity.Property(e => e.PriceUsd)
                    .HasColumnType("money")
                    .HasColumnName("PriceUSD");

                entity.Property(e => e.Request)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('no')");

                entity.Property(e => e.Type)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.BuyerAssets)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FKBuyerAssetsCountryId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BuyerAssets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FKBuyerAssetsUserId");
            });

            modelBuilder.Entity<CurrencyConversion>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("PKCurrencyConversion");

                entity.ToTable("CurrencyConversion");

                entity.HasIndex(e => e.CountryName, "NameCurrencyConversion")
                    .IsUnique();

                entity.Property(e => e.CountryId).ValueGeneratedNever();

                entity.Property(e => e.CountryName)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InsurerDetail>(entity =>
            {
                entity.HasKey(e => e.InsurerId)
                    .HasName("PKInsurerDetails");

                entity.ToTable("Insurer.Details");

                entity.Property(e => e.InsurerId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Commission).HasDefaultValueSql("((0.0))");

                entity.Property(e => e.NoOfProducts).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Insurer)
                    .WithOne(p => p.InsurerDetail)
                    .HasForeignKey<InsurerDetail>(d => d.InsurerId)
                    .HasConstraintName("FK__Insurer.D__Insur__37A5467C");
            });

            modelBuilder.Entity<PaymentBuyer>(entity =>
            {
                entity.HasKey(e => e.PolicyId)
                    .HasName("PKPaymentBuyer");

                entity.ToTable("PaymentBuyer");

                entity.Property(e => e.PolicyId).ValueGeneratedNever();

                entity.Property(e => e.PaidStatus)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('false')");

                entity.HasOne(d => d.Policy)
                    .WithOne(p => p.PaymentBuyer)
                    .HasForeignKey<PaymentBuyer>(d => d.PolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaymentBu__Polic__47DBAE45");
            });

            modelBuilder.Entity<PolicyDetail>(entity =>
            {
                entity.HasKey(e => e.PolicyId)
                    .HasName("PKPolicyDetails");

                entity.Property(e => e.BrokerId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Feedback).IsUnicode(false);

                entity.Property(e => e.InsurerId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LumpSum).HasColumnType("money");

                entity.Property(e => e.MaturityAmount).HasColumnType("money");

                entity.Property(e => e.PolicyStatus)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Premium).HasColumnType("money");

                entity.Property(e => e.ReviewStatus)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.PolicyDetails)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("FKPolicyDetailsAssetId");

                entity.HasOne(d => d.Broker)
                    .WithMany(p => p.PolicyDetails)
                    .HasForeignKey(d => d.BrokerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPolicyDetailsBrokerId");

                entity.HasOne(d => d.Insurer)
                    .WithMany(p => p.PolicyDetails)
                    .HasForeignKey(d => d.InsurerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPolicyDetailsInsurerId");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PKUserDetails");

                entity.ToTable("User.Details");

                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LicenseId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
