using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Placeholder.Data.Sql.Models;

namespace Placeholder.Data.Sql
{
    public partial class PlaceholderContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<GlobalSetting> GlobalSettings { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<ShopAccount> ShopAccounts { get; set; }
        public virtual DbSet<ShopIsolated> ShopIsolateds { get; set; }
        public virtual DbSet<ShopSetting> ShopSettings { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<Widget> Widgets { get; set; }

        public PlaceholderContext(DbContextOptions<PlaceholderContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.account_id);

                entity.ToTable("Account");

                entity.HasIndex(e => e.email, "UK_account_email")
                    .IsUnique();

                entity.HasIndex(e => e.api_key, "UK_account_key")
                    .IsUnique();

                entity.Property(e => e.account_id).ValueGeneratedNever();

                entity.Property(e => e.account_display).HasMaxLength(150);

                entity.Property(e => e.api_key)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.api_secret)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.deleted_utc).HasPrecision(0);

                entity.Property(e => e.email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.email_verify_token)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.email_verify_utc).HasPrecision(0);

                entity.Property(e => e.entitlements).HasMaxLength(250);

                entity.Property(e => e.first_name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.last_login_platform).HasMaxLength(250);

                entity.Property(e => e.last_login_utc).HasPrecision(0);

                entity.Property(e => e.last_name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.password_changed_utc).HasPrecision(0);

                entity.Property(e => e.password_reset_token).HasMaxLength(50);

                entity.Property(e => e.password_reset_utc).HasPrecision(0);

                entity.Property(e => e.password_salt)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.single_login_token).HasMaxLength(50);

                entity.Property(e => e.single_login_token_expire_utc).HasPrecision(0);

                entity.Property(e => e.sync_agent).HasMaxLength(50);

                entity.Property(e => e.sync_attempt_utc).HasPrecision(0);

                entity.Property(e => e.sync_hydrate_utc).HasPrecision(0);

                entity.Property(e => e.sync_invalid_utc).HasPrecision(0);

                entity.Property(e => e.sync_success_utc).HasPrecision(0);

                entity.Property(e => e.timezone).HasMaxLength(128);

                entity.Property(e => e.updated_utc).HasPrecision(0);

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.asset_id_avatar);
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.asset_id);

                entity.ToTable("Asset");

                entity.Property(e => e.asset_id).ValueGeneratedNever();

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.encode_attempt_utc).HasPrecision(0);

                entity.Property(e => e.encode_identifier).HasMaxLength(50);

                entity.Property(e => e.encode_status).HasMaxLength(50);

                entity.Property(e => e.public_url).HasMaxLength(512);

                entity.Property(e => e.raw_url).HasMaxLength(512);

                entity.Property(e => e.relative_path).HasMaxLength(512);

                entity.Property(e => e.resize_attempt_utc).HasPrecision(0);

                entity.Property(e => e.resize_mode).HasMaxLength(20);

                entity.Property(e => e.resize_status).HasMaxLength(50);

                entity.Property(e => e.thumb_large_dimensions).HasMaxLength(10);

                entity.Property(e => e.thumb_large_url).HasMaxLength(512);

                entity.Property(e => e.thumb_medium_dimensions).HasMaxLength(10);

                entity.Property(e => e.thumb_medium_url).HasMaxLength(512);

                entity.Property(e => e.thumb_small_dimensions).HasMaxLength(10);

                entity.Property(e => e.thumb_small_url).HasMaxLength(512);

                entity.Property(e => e.updated_utc).HasPrecision(0);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.company_id);

                entity.ToTable("Company");

                entity.Property(e => e.company_id).ValueGeneratedNever();

                entity.Property(e => e.company_name).HasMaxLength(150);

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.deleted_utc).HasPrecision(0);

                entity.Property(e => e.sync_agent).HasMaxLength(50);

                entity.Property(e => e.sync_attempt_utc).HasPrecision(0);

                entity.Property(e => e.sync_hydrate_utc).HasPrecision(0);

                entity.Property(e => e.sync_invalid_utc).HasPrecision(0);

                entity.Property(e => e.sync_success_utc).HasPrecision(0);

                entity.Property(e => e.updated_utc).HasPrecision(0);

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.shop_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<GlobalSetting>(entity =>
            {
                entity.HasKey(e => e.global_setting_id);

                entity.ToTable("GlobalSetting");

                entity.HasIndex(e => e.name, "UK_singleglobal")
                    .IsUnique();

                entity.Property(e => e.global_setting_id).ValueGeneratedNever();

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.shop_id);

                entity.ToTable("Shop");

                entity.Property(e => e.shop_id).ValueGeneratedNever();

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.deleted_utc).HasPrecision(0);

                entity.Property(e => e.private_domain)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.public_domain)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.shop_name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.sync_agent).HasMaxLength(50);

                entity.Property(e => e.sync_attempt_utc).HasPrecision(0);

                entity.Property(e => e.sync_hydrate_utc).HasPrecision(0);

                entity.Property(e => e.sync_invalid_utc).HasPrecision(0);

                entity.Property(e => e.sync_success_utc).HasPrecision(0);

                entity.Property(e => e.updated_utc).HasPrecision(0);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.tenant_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ShopAccount>(entity =>
            {
                entity.HasKey(e => e.shop_account_id);

                entity.ToTable("ShopAccount");

                entity.Property(e => e.shop_account_id).ValueGeneratedNever();

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.deleted_utc).HasPrecision(0);

                entity.Property(e => e.sync_agent).HasMaxLength(50);

                entity.Property(e => e.sync_attempt_utc).HasPrecision(0);

                entity.Property(e => e.sync_hydrate_utc).HasPrecision(0);

                entity.Property(e => e.sync_invalid_utc).HasPrecision(0);

                entity.Property(e => e.sync_success_utc).HasPrecision(0);

                entity.Property(e => e.updated_utc).HasPrecision(0);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ShopAccounts)
                    .HasForeignKey(d => d.account_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopAccounts)
                    .HasForeignKey(d => d.shop_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ShopIsolated>(entity =>
            {
                entity.HasKey(e => e.shop_id);

                entity.ToTable("ShopIsolated");

                entity.Property(e => e.shop_id).ValueGeneratedNever();

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.deleted_utc).HasPrecision(0);

                entity.Property(e => e.sync_agent).HasMaxLength(50);

                entity.Property(e => e.sync_attempt_utc).HasPrecision(0);

                entity.Property(e => e.sync_hydrate_utc).HasPrecision(0);

                entity.Property(e => e.sync_invalid_utc).HasPrecision(0);

                entity.Property(e => e.sync_success_utc).HasPrecision(0);

                entity.Property(e => e.updated_utc).HasPrecision(0);

                entity.HasOne(d => d.Shop)
                    .WithOne(p => p.ShopIsolated)
                    .HasForeignKey<ShopIsolated>(d => d.shop_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ShopSetting>(entity =>
            {
                entity.HasKey(e => e.shop_setting_id);

                entity.ToTable("ShopSetting");

                entity.HasIndex(e => new { e.shop_id, e.name }, "UK_shop_setting")
                    .IsUnique();

                entity.Property(e => e.shop_setting_id).ValueGeneratedNever();

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.deleted_utc).HasPrecision(0);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.sync_agent).HasMaxLength(50);

                entity.Property(e => e.sync_attempt_utc).HasPrecision(0);

                entity.Property(e => e.sync_hydrate_utc).HasPrecision(0);

                entity.Property(e => e.sync_invalid_utc).HasPrecision(0);

                entity.Property(e => e.sync_success_utc).HasPrecision(0);

                entity.Property(e => e.updated_utc).HasPrecision(0);

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopSettings)
                    .HasForeignKey(d => d.shop_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.tenant_id);

                entity.ToTable("Tenant");

                entity.HasIndex(e => e.tenant_code, "UK_tenant_code")
                    .IsUnique();

                entity.Property(e => e.tenant_id).ValueGeneratedNever();

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.tenant_code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.tenant_name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.updated_utc).HasPrecision(0);
            });

            modelBuilder.Entity<Widget>(entity =>
            {
                entity.HasKey(e => e.widget_id);

                entity.ToTable("Widget");

                entity.Property(e => e.widget_id).ValueGeneratedNever();

                entity.Property(e => e.created_utc).HasPrecision(0);

                entity.Property(e => e.deleted_utc).HasPrecision(0);

                entity.Property(e => e.stamp_utc).HasPrecision(0);

                entity.Property(e => e.sync_agent).HasMaxLength(50);

                entity.Property(e => e.sync_attempt_utc).HasPrecision(0);

                entity.Property(e => e.sync_hydrate_utc).HasPrecision(0);

                entity.Property(e => e.sync_invalid_utc).HasPrecision(0);

                entity.Property(e => e.sync_success_utc).HasPrecision(0);

                entity.Property(e => e.updated_utc).HasPrecision(0);

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Widgets)
                    .HasForeignKey(d => d.shop_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
