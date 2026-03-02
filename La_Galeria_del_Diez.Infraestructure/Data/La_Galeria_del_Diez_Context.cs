    using System;
using System.Collections.Generic;
using La_Galeria_del_Diez.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace La_Galeria_del_Diez.Infraestructure.Data;

public partial class La_Galeria_del_Diez_Context : DbContext
{
    public La_Galeria_del_Diez_Context(DbContextOptions<La_Galeria_del_Diez_Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Auction> Auction { get; set; }

    public virtual DbSet<AuctionableObject> AuctionableObject { get; set; }

    public virtual DbSet<Bidding> Bidding { get; set; }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<Image> Image { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<State> State { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auction__3214EC07EAA96921");

            entity.Property(e => e.AuctionWinner).HasColumnName("Auction_Winner");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Base_Price");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.IdObject).HasColumnName("Id_Object");
            entity.Property(e => e.IdState).HasColumnName("Id_State");
            entity.Property(e => e.IdUser).HasColumnName("Id_User");
            entity.Property(e => e.MinIncrement)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Min_Increment");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");

            entity.HasOne(d => d.IdObjectNavigation).WithMany(p => p.Auction)
                .HasForeignKey(d => d.IdObject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auction_Object");

            entity.HasOne(d => d.IdStateNavigation).WithMany(p => p.Auction)
                .HasForeignKey(d => d.IdState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auction_State");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Auction)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auction_User");

            entity.HasOne(d => d.Winner).WithMany()
                .HasForeignKey(d => d.AuctionWinner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auction_Winner");
        });

        modelBuilder.Entity<AuctionableObject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auctiona__3214EC0763CB7994");

            entity.ToTable("Auctionable_Object");

            entity.Property(e => e.Condition)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IdState).HasColumnName("Id_State");
            entity.Property(e => e.IdUser).HasColumnName("Id_User");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Registration_Date");

            entity.HasOne(d => d.IdStateNavigation).WithMany(p => p.AuctionableObject)
                .HasForeignKey(d => d.IdState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Object_State");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.AuctionableObject)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Object_User");

            entity.HasMany(d => d.IdCategory).WithMany(p => p.IdObject)
                .UsingEntity<Dictionary<string, object>>(
                    "ObjectCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("IdCategory")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ObjectCategory_Category"),
                    l => l.HasOne<AuctionableObject>().WithMany()
                        .HasForeignKey("IdObject")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ObjectCategory_Object"),
                    j =>
                    {
                        j.HasKey("IdObject", "IdCategory").HasName("PK__Object_C__33FE25FEEA895D6B");
                        j.ToTable("Object_Category");
                        j.IndexerProperty<int>("IdObject").HasColumnName("Id_Object");
                        j.IndexerProperty<int>("IdCategory").HasColumnName("Id_Category");
                    });
        });

        modelBuilder.Entity<Bidding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bidding__3214EC07626B5183");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IdAuction).HasColumnName("Id_Auction");
            entity.Property(e => e.IdUser).HasColumnName("Id_User");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Payment_Method");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Registration_date");

            entity.HasOne(d => d.IdAuctionNavigation).WithMany(p => p.Bidding)
                .HasForeignKey(d => d.IdAuction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bidding_Auction");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Bidding)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bidding_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07FA4A3054");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC07A292D694");

            entity.Property(e => e.IdObject).HasColumnName("Id_object");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Registration_Date");

            entity.HasOne(d => d.IdObjectNavigation).WithMany(p => p.Image)
                .HasForeignKey(d => d.IdObject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Image_Object");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC07FE8F64A1");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__State__3214EC072C7804E4");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07BD237469");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534D1094F39").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Registration_Date");
            entity.Property(e => e.UserState)
                .HasDefaultValue(true)
                .HasColumnName("User_State");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.User)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
