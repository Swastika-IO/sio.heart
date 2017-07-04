using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TTS.Lib.Models;

namespace TTS.Lib.Migrations
{
    [DbContext(typeof(ttsContext))]
    [Migration("20170701041353_AddCultureIcon")]
    partial class AddCultureIcon
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TTS.Lib.Models.AspNetRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("RoleId")
                        .HasName("IX_AspNetRoleClaims_RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetRoles", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .HasName("IX_AspNetUserClaims_UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetUserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(450);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(450);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("PK_AspNetUserLogins");

                    b.HasIndex("UserId")
                        .HasName("IX_AspNetUserLogins_UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetUserRoles", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.Property<string>("RoleId")
                        .HasMaxLength(450);

                    b.HasKey("UserId", "RoleId")
                        .HasName("PK_AspNetUserRoles");

                    b.HasIndex("RoleId")
                        .HasName("IX_AspNetUserRoles_RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetUsers", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Avatar");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("Culture");

                    b.Property<DateTime?>("Dob")
                        .HasColumnName("DOB");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<bool>("IsActived")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<DateTime>("JoinDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("'0001-01-01T00:00:00.000'");

                    b.Property<DateTime>("LastModified")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("'0001-01-01T00:00:00.000'");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("NickName");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("RegisterType");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetUserTokens", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(450);

                    b.Property<string>("Name")
                        .HasMaxLength(450);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("PK_AspNetUserTokens");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TTS.Lib.Models.TtsArticle", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("Title")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("Culture");

                    b.ToTable("TTS_Article");
                });

            modelBuilder.Entity("TTS.Lib.Models.TTSBanner", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("Alias")
                        .HasMaxLength(250);

                    b.Property<string>("CultureCode")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("N'en'")
                        .HasMaxLength(10);

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Url")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("N'#'")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("TTS_Banner");
                });

            modelBuilder.Entity("TTS.Lib.Models.TtsCopy", b =>
                {
                    b.Property<string>("Culture")
                        .HasMaxLength(10);

                    b.Property<string>("Keyword")
                        .HasMaxLength(250);

                    b.Property<string>("Note")
                        .HasMaxLength(250);

                    b.Property<string>("Value");

                    b.HasKey("Culture", "Keyword")
                        .HasName("PK_TTX_Copy");

                    b.ToTable("TTS_Copy");
                });

            modelBuilder.Entity("TTS.Lib.Models.TtsCulture", b =>
                {
                    b.Property<string>("Specificulture")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10);

                    b.Property<string>("Alias")
                        .HasColumnType("nchar(10)");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<string>("FullName")
                        .HasMaxLength(150);

                    b.Property<string>("Lcid")
                        .HasColumnName("LCID")
                        .HasColumnType("nchar(10)");

                    b.HasKey("Specificulture")
                        .HasName("PK_TTS_Culture");

                    b.ToTable("TTS_Culture");
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetRoleClaims", b =>
                {
                    b.HasOne("TTS.Lib.Models.AspNetRoles", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetUserClaims", b =>
                {
                    b.HasOne("TTS.Lib.Models.AspNetUsers", "User")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetUserLogins", b =>
                {
                    b.HasOne("TTS.Lib.Models.AspNetUsers", "User")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TTS.Lib.Models.AspNetUserRoles", b =>
                {
                    b.HasOne("TTS.Lib.Models.AspNetRoles", "Role")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TTS.Lib.Models.AspNetUsers", "User")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TTS.Lib.Models.TtsArticle", b =>
                {
                    b.HasOne("TTS.Lib.Models.TtsCulture", "CultureNavigation")
                        .WithMany("TtsArticle")
                        .HasForeignKey("Culture")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TTS.Lib.Models.TtsCopy", b =>
                {
                    b.HasOne("TTS.Lib.Models.TtsCulture", "CultureNavigation")
                        .WithMany("TtsCopy")
                        .HasForeignKey("Culture")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
