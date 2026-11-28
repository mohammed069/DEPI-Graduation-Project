using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MissingPersons.DAL.Models;

public partial class LostFoundContext : IdentityDbContext<User,IdentityRole<int>,int>
{
    public LostFoundContext()
    {
    }

    public LostFoundContext(DbContextOptions<LostFoundContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<ItemDetail> ItemDetails { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PersonDetail> PersonDetails { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportFlag> ReportFlags { get; set; }

    public virtual DbSet<ReportImage> ReportImages { get; set; }

    public virtual DbSet<UserLocationPreference> UserLocationPreferences { get; set; }

    public virtual DbSet<UserVerification> UserVerifications { get; set; }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC070B6A509D");

            entity.ToTable("Conversation");

            entity.HasIndex(e => e.MatchId, "UQ__Conversa__4218C816521AB6E2").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Match).WithOne(p => p.Conversation)
                .HasForeignKey<Conversation>(d => d.MatchId)
                .HasConstraintName("FK_Conversation_Match");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ItemCate__3214EC07642B6C68");

            entity.ToTable("ItemCategory");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_ItemCategory_ParentCategory");
        });

        modelBuilder.Entity<ItemDetail>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__ItemDeta__D5BD4805843D0FC5");

            entity.ToTable("ItemDetail");

            entity.Property(e => e.ReportId).ValueGeneratedNever();
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.Property(e => e.Size).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.ItemDetails)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_ItemDetail_Category");

            entity.HasOne(d => d.Report).WithOne(p => p.ItemDetail)
                .HasForeignKey<ItemDetail>(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemDetail_Report");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Match__3214EC073048DEA9");

            entity.ToTable("Match");

            entity.HasIndex(e => new { e.LostReportId, e.FoundReportId }, "Match_index_0").IsUnique();

            entity.Property(e => e.ConfirmedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.FoundReport).WithMany(p => p.MatchFoundReports)
                .HasForeignKey(d => d.FoundReportId)
                .HasConstraintName("FK_Match_FoundReport");

            entity.HasOne(d => d.LostReport).WithMany(p => p.MatchLostReports)
                .HasForeignKey(d => d.LostReportId)
                .HasConstraintName("FK_Match_LostReport");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC07027048CF");

            entity.ToTable("Message");

            entity.HasIndex(e => e.ConversationId, "IX_Message_ConversationId");

            entity.HasIndex(e => e.SenderId, "IX_Message_SenderId");

            entity.Property(e => e.SentAt).HasColumnType("datetime");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("FK_Message_Conversation");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_Message_Sender");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0751569CED");

            entity.ToTable("Notification");

            entity.HasIndex(e => e.UserId, "IX_Notification_UserId");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ReferenceType).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<PersonDetail>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__PersonDe__D5BD48050B29E097");

            entity.ToTable("PersonDetail");

            entity.Property(e => e.ReportId).ValueGeneratedNever();
            entity.Property(e => e.ContactPhoneForFamily).HasMaxLength(20);
            entity.Property(e => e.EyeColor).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.HairColor).HasMaxLength(50);
            entity.Property(e => e.PersonName).HasMaxLength(150);

            entity.HasOne(d => d.Report).WithOne(p => p.PersonDetail)
                .HasForeignKey<PersonDetail>(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonDetail_Report");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07D42C045E");

            entity.ToTable("Report");

            entity.HasIndex(e => e.City, "IX_Report_City");

            entity.HasIndex(e => e.EventDate, "IX_Report_EventDate");

            entity.HasIndex(e => new { e.ReportType, e.TargetType }, "IX_Report_ReportType_TargetType");

            entity.HasIndex(e => e.Status, "IX_Report_Status");

            entity.HasIndex(e => e.UserId, "IX_Report_UserId");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EventDate).HasColumnType("datetime");
            entity.Property(e => e.ReportType).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TargetType).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Report_User");
        });

        modelBuilder.Entity<ReportFlag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReportFl__3214EC07212CBD50");

            entity.ToTable("ReportFlag");

            entity.HasIndex(e => e.ReportId, "IX_ReportFlag_ReportId");

            entity.Property(e => e.ReviewedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Report).WithMany(p => p.ReportFlags)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("FK_ReportFlag_Report");

            entity.HasOne(d => d.ReportedByUser).WithMany(p => p.ReportFlagReportedByUsers)
                .HasForeignKey(d => d.ReportedByUserId)
                .HasConstraintName("FK_ReportFlag_ReportedByUser");

            entity.HasOne(d => d.ReviewedByAdmin).WithMany(p => p.ReportFlagReviewedByAdmins)
                .HasForeignKey(d => d.ReviewedByAdminId)
                .HasConstraintName("FK_ReportFlag_ReviewedByAdmin");
        });

        modelBuilder.Entity<ReportImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReportIm__3214EC073F593345");

            entity.ToTable("ReportImage");

            entity.HasIndex(e => e.ReportId, "IX_ReportImage_ReportId");

            entity.Property(e => e.ImageHash).HasMaxLength(256);
            entity.Property(e => e.ImageUrl).HasMaxLength(1000);
            entity.Property(e => e.UploadedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportImages)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("FK_ReportImage_Report");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC076B78606A");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D105345E491673").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(1000);
        });

        modelBuilder.Entity<UserLocationPreference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserLoca__3214EC074D4272CC");

            entity.ToTable("UserLocationPreference");

            entity.HasIndex(e => e.UserId, "UQ__UserLoca__1788CC4DE339D460").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.UserLocationPreference)
                .HasForeignKey<UserLocationPreference>(d => d.UserId)
                .HasConstraintName("FK_UserLocationPreference_User");
        });

        modelBuilder.Entity<UserVerification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserVeri__3214EC073C40C52A");

            entity.ToTable("UserVerification");

            entity.HasIndex(e => e.UserId, "IX_UserVerification_UserId");

            entity.Property(e => e.DocumentUrl).HasMaxLength(1000);
            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.ReviewedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.ReviewedByAdmin).WithMany(p => p.UserVerificationReviewedByAdmins)
                .HasForeignKey(d => d.ReviewedByAdminId)
                .HasConstraintName("FK_UserVerification_ReviewedByAdmin");

            entity.HasOne(d => d.User).WithMany(p => p.UserVerificationUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserVerification_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
