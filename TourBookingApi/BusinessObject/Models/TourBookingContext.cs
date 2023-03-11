using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusinessObject.Models
{
    public partial class TourBookingContext : DbContext
    {
        public TourBookingContext()
        {
        }

        public TourBookingContext(DbContextOptions<TourBookingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Destination> Destinations { get; set; } = null!;
        public virtual DbSet<DestinationImage> DestinationImages { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Tour> Tours { get; set; } = null!;
        public virtual DbSet<TourDetail> TourDetails { get; set; } = null!;
        public virtual DbSet<TourGuide> TourGuides { get; set; } = null!;
        public virtual DbSet<TourPrice> TourPrices { get; set; } = null!;
        public virtual DbSet<Transportation> Transportations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=PHUOC-LAPTOP\\PHUOCLT;database=TourBooking;uid=sa;pwd=1234567890;Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("phone");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("date")
                    .HasColumnName("booking_date");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.NumAdults).HasColumnName("num_adults");

                entity.Property(e => e.NumChildren).HasColumnName("num_children");

                entity.Property(e => e.NumInfants).HasColumnName("num_infants");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("total_price");

                entity.Property(e => e.TourId).HasColumnName("tour_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_Account");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__tour_id__628FA481");
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.ToTable("Destination");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Region)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("region");
            });

            modelBuilder.Entity<DestinationImage>(entity =>
            {
                entity.ToTable("DestinationImage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DestinationId).HasColumnName("destination_id");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.DestinationImages)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Destinati__desti__4BAC3F29");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("payment_amount");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("payment_date");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("payment_method");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__booking__66603565");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Role");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.Role1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.HasOne(d => d.Account)
                    .WithMany()
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Role__accountID__5DCAEF64");
            });

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.ToTable("Tour");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.TourCapacity).HasColumnName("tour_capacity");

                entity.Property(e => e.TourDuration).HasColumnName("tour_duration");

                entity.Property(e => e.TourGuideId).HasColumnName("tour_guide_id");

                entity.Property(e => e.TourName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tour_name");
            });

            modelBuilder.Entity<TourDetail>(entity =>
            {
                entity.ToTable("TourDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Departure)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("departure");

                entity.Property(e => e.DestinationId).HasColumnName("destination_id");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.ExpiredDate)
                    .HasColumnType("date")
                    .HasColumnName("expired_date");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.TourDescription)
                    .HasColumnType("text")
                    .HasColumnName("tour_description");

                entity.Property(e => e.TourId).HasColumnName("tour_id");

                entity.Property(e => e.TransportationId).HasColumnName("transportation_id");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.TourDetails)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TourDetai__desti__534D60F1");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.TourDetails)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TourDetai__tour___52593CB8");

                entity.HasOne(d => d.Transportation)
                    .WithMany(p => p.TourDetails)
                    .HasForeignKey(d => d.TransportationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TourDetai__trans__5441852A");
            });

            modelBuilder.Entity<TourGuide>(entity =>
            {
                entity.ToTable("TourGuide");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TourGuideAge).HasColumnName("tour_guide_age");

                entity.Property(e => e.TourGuideAva).HasColumnName("tour_guide_ava");

                entity.Property(e => e.TourGuideBio)
                    .HasColumnType("text")
                    .HasColumnName("tour_guide_bio");

                entity.Property(e => e.TourGuideEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tour_guide_email");

                entity.Property(e => e.TourGuideLanguageSpoken)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tour_guide_language_spoken");

                entity.Property(e => e.TourGuideName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tour_guide_name");

                entity.Property(e => e.TourGuidePhone)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tour_guide_phone");

                entity.Property(e => e.TourId).HasColumnName("tour_id");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.TourGuides)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TourGuide__tour___571DF1D5");
            });

            modelBuilder.Entity<TourPrice>(entity =>
            {
                entity.ToTable("TourPrice");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PriceAdults)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price_adults");

                entity.Property(e => e.PriceChildren)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price_children");

                entity.Property(e => e.PriceInfants)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price_infants");

                entity.Property(e => e.TourId).HasColumnName("tour_id");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.TourPrices)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TourPrice__tour___59FA5E80");
            });

            modelBuilder.Entity<Transportation>(entity =>
            {
                entity.ToTable("Transportation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TransportationDescription)
                    .HasColumnType("text")
                    .HasColumnName("transportation_description");

                entity.Property(e => e.TransportationType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("transportation_type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
