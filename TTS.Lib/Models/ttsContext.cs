using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TTS.Lib.Models
{
    public partial class ttsContext : DbContext
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<TtsArticle> TtsArticle { get; set; }
        public virtual DbSet<TtsBanner> TtsBanner { get; set; }
        public virtual DbSet<TtsComment> TtsComment { get; set; }
        public virtual DbSet<TtsCopy> TtsCopy { get; set; }
        public virtual DbSet<TtsCulture> TtsCulture { get; set; }
        public virtual DbSet<TtsMenu> TtsMenu { get; set; }
        public virtual DbSet<TtsMenuArticle> TtsMenuArticle { get; set; }
        public virtual DbSet<TtsMenuMenu> TtsMenuMenu { get; set; }
        public virtual DbSet<TtsParameter> TtsParameter { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=.\sqlexpress;Database=tts;UID=sa;Pwd=1234qwe@;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetUserRoles_RoleId");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.CountryId).HasDefaultValueSql("0");

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.IsActived).HasDefaultValueSql("0");

                entity.Property(e => e.JoinDate).HasDefaultValueSql("'0001-01-01T00:00:00.000'");

                entity.Property(e => e.LastModified).HasDefaultValueSql("'0001-01-01T00:00:00.000'");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<TtsArticle>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PK_TTS_Article");

                entity.ToTable("TTS_Article");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Hot).HasDefaultValueSql("0");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");

                entity.Property(e => e.IsVisible).HasDefaultValueSql("1");

                entity.Property(e => e.Menus).HasMaxLength(50);

                entity.Property(e => e.Sename).HasColumnName("SEName");

                entity.Property(e => e.Seodescription).HasColumnName("SEODescription");

                entity.Property(e => e.Seokeywords).HasColumnName("SEOKeywords");

                entity.Property(e => e.Seotitle).HasColumnName("SEOTitle");

                entity.Property(e => e.Type).HasDefaultValueSql("0");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.TtsArticle)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TTS_Article_TTS_Culture");
            });

            modelBuilder.Entity<TtsBanner>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PK_TTS_Banner");

                entity.ToTable("TTS_Banner");

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Specificulture)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("N'en'");

                entity.Property(e => e.Alias).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("0");

                entity.Property(e => e.ModifiedBy).HasMaxLength(450);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("N'#'");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.TtsBanner)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TTS_Banner_TTS_Culture");
            });

            modelBuilder.Entity<TtsComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK_Comments");

                entity.ToTable("TTS_Comment");

                entity.Property(e => e.CommentId).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EditedBy).HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FullName).HasMaxLength(250);
            });

            modelBuilder.Entity<TtsCopy>(entity =>
            {
                entity.HasKey(e => new { e.Culture, e.Keyword })
                    .HasName("PK_TTX_Copy");

                entity.ToTable("TTS_Copy");

                entity.Property(e => e.Culture).HasMaxLength(10);

                entity.Property(e => e.Keyword).HasMaxLength(250);

                entity.Property(e => e.Note).HasMaxLength(250);
            });

            modelBuilder.Entity<TtsCulture>(entity =>
            {
                entity.ToTable("TTS_Culture");

                entity.HasIndex(e => e.Specificulture)
                    .HasName("IX_TTS_Culture")
                    .IsUnique();

                entity.Property(e => e.Alias).HasMaxLength(150);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.FullName).HasMaxLength(150);

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Lcid)
                    .HasColumnName("LCID")
                    .HasMaxLength(50);

                entity.Property(e => e.Specificulture)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<TtsMenu>(entity =>
            {
                entity.HasKey(e => new { e.MenuId, e.Specificulture })
                    .HasName("PK_TTS_Menu");

                entity.ToTable("TTS_Menu");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Level).HasDefaultValueSql("0");

                entity.Property(e => e.ParentMenuId).HasDefaultValueSql("0");

                entity.Property(e => e.Sename).HasColumnName("SEName");

                entity.Property(e => e.Seodescription).HasColumnName("SEODescription");

                entity.Property(e => e.Seokeywords).HasColumnName("SEOKeywords");

                entity.Property(e => e.Seotitle).HasColumnName("SEOTitle");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.TtsMenu)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .HasConstraintName("FK_TTS_Menu_TTS_Culture");
            });

            modelBuilder.Entity<TtsMenuArticle>(entity =>
            {
                entity.HasKey(e => new { e.ArticleId, e.MenuId, e.Specificulture })
                    .HasName("PK_TTS_Menu_Article");

                entity.ToTable("TTS_Menu_Article");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.HasOne(d => d.TtsArticle)
                    .WithMany(p => p.TtsMenuArticle)
                    .HasForeignKey(d => new { d.ArticleId, d.Specificulture })
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TTS_Menu_Article_TTS_Article1");

                entity.HasOne(d => d.TtsMenu)
                    .WithMany(p => p.TtsMenuArticle)
                    .HasForeignKey(d => new { d.MenuId, d.Specificulture })
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TTS_Menu_Article_TTS_Menu");
            });

            modelBuilder.Entity<TtsMenuMenu>(entity =>
            {
                entity.HasKey(e => new { e.MenuId, e.ParentId, e.Specificulture })
                    .HasName("PK_TTS_Menu_Menu");

                entity.ToTable("TTS_Menu_Menu");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.HasOne(d => d.TtsMenu)
                    .WithMany(p => p.TtsMenuMenuTtsMenu)
                    .HasForeignKey(d => new { d.MenuId, d.Specificulture })
                    .HasConstraintName("FK_TTS_Menu_Menu_TTS_Menu");

                entity.HasOne(d => d.TtsMenuNavigation)
                    .WithMany(p => p.TtsMenuMenuTtsMenuNavigation)
                    .HasForeignKey(d => new { d.ParentId, d.Specificulture })
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TTS_Menu_Menu_TTS_Menu1");
            });

            modelBuilder.Entity<TtsParameter>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK_Parameters");

                entity.ToTable("TTS_Parameter");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Value).IsRequired();
            });
        }
    }
}