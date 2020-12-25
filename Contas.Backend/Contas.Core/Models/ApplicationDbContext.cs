using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Contas.Core.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        public virtual DbSet<TbContas> Contas { get; set; }
        public virtual DbSet<JwtTokens> JwtTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=localhost;Database=my_db_local;Uid=root;Pwd=root");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(56).IsRequired();
                entity.Property(e => e.NormalizedName).HasMaxLength(254);
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

            });

            var guidAspNetRole = "A9F8316C-0749-4AAF-8AA5-F19759A5B469";
            var guidAspNetUser = "30920176-94A7-44C4-997A-116CABF5709F";

            modelBuilder.Entity<AspNetRoles>().HasData(
                new AspNetRoles { 
                    Id = guidAspNetRole, 
                    Name = "Admin", 
                    NormalizedName = "ADMIN", 
                    ConcurrencyStamp = Guid.NewGuid().ToString(), 
                    DataCadastro = DateTime.Now,
                    Ativo = true,
                    DataUltimaAlteracao = DateTime.Now,
                }
            );

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.UserId).HasMaxLength(56).IsRequired();
                entity.Property(e => e.RoleId).HasMaxLength(56).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasMaxLength(56).IsRequired();
                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(56).IsRequired();
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);


                entity.Property(e => e.NormalizedEmail).HasMaxLength(254);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(254);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.Property(e => e.AlterPassword).HasDefaultValue(false);
            });

            var adminSeed = new AspNetUsers
            {
                Id = guidAspNetUser,
                Ativo = true,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                DataCadastro = DateTime.Now,
                DataUltimaAlteracao = DateTime.Now,
                FullName = "Admin",
                NormalizedUserName = "ADMIN",
                UserName = "admin",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            PasswordHasher<AspNetUsers> ph = new PasswordHasher<AspNetUsers>();
            adminSeed.PasswordHash = ph.HashPassword(adminSeed, "admin");

            modelBuilder.Entity<AspNetUsers>().HasData(adminSeed);

            modelBuilder.Entity<AspNetUserRoles>().HasData(
                new AspNetUserRoles
                {
                    RoleId = guidAspNetRole,
                    UserId = guidAspNetUser
                }
            );

            modelBuilder.Entity<TbContas>(entity =>
            {
                entity.HasKey(e => e.ContaId);

                entity.Property(e => e.Nome).IsRequired();
                entity.Property(e => e.ValorOriginal).IsRequired();
                entity.Property(e => e.ValorMulta).IsRequired();
                entity.Property(e => e.ValorFinal).IsRequired();
                entity.Property(e => e.Ativo).IsRequired();
                entity.Property(e => e.DataCadastro).IsRequired();
                entity.Property(e => e.DataUltimaAlteracao).IsRequired();
                entity.Property(e => e.Vencimento).IsRequired();
                entity.Property(e => e.Pagamento).IsRequired();
                entity.Property(e => e.QntDiasAtraso).IsRequired();
            });

            modelBuilder.Entity<JwtTokens>(entity =>
            {
                entity.HasKey(e => e.JwtTokenId);

                entity.Property(e => e.UsuarioId).IsRequired();
                entity.Property(e => e.Token).IsRequired();
                entity.Property(e => e.DataExpiracao).IsRequired();
                entity.Property(e => e.IpCriacao).IsRequired();
                entity.Property(e => e.IpRevogacao).IsRequired(false);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.JwtTokens)
                    .HasForeignKey(d => d.UsuarioId);
            });

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
