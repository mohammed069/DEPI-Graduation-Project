using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizAttempt> QuizAttempts { get; set; }

    public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourseEnrollment> UserCourseEnrollments { get; set; }

    public virtual DbSet<UserCourseTeach> UserCourseTeaches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__COURSE__C92D7187076C4B8C");

            entity.ToTable("COURSE");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("CourseID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__OPTIONS__92C7A1DF18E748E1");

            entity.ToTable("OPTIONS");

            entity.HasIndex(e => e.QuestionId, "IX_Options_QuestionID");

            entity.HasIndex(e => new { e.OptionId, e.QuestionId }, "UQ_Options_Option_Question").IsUnique();

            entity.HasIndex(e => e.QuestionId, "UX_OPTIONS_OneCorrectAnswer")
                .IsUnique()
                .HasFilter("([IsCorrect]=(1))");

            entity.Property(e => e.OptionId)
                .ValueGeneratedNever()
                .HasColumnName("OptionID");
            entity.Property(e => e.OptionText)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("Option_Text");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

            entity.HasOne(d => d.Question).WithMany(p => p.Option)
                .HasForeignKey<Option>(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Options_Question");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__QUESTION__0DC06F8C3F604EE6");

            entity.ToTable("QUESTION");

            entity.HasIndex(e => e.CourseId, "IX_Question_CourseID");

            entity.Property(e => e.QuestionId)
                .ValueGeneratedNever()
                .HasColumnName("QuestionID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Marks).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.QuestionText)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Question_Text");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Course).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Question_Course");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId).HasName("PK__QUIZ__8B42AE6E8A0B02D2");

            entity.ToTable("QUIZ");

            entity.HasIndex(e => e.CourseId, "IX_Quiz_CourseID");

            entity.HasIndex(e => e.CreatedBy, "IX_Quiz_CreatedBy");

            entity.Property(e => e.QuizId)
                .ValueGeneratedNever()
                .HasColumnName("QuizID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.StartTime).HasColumnName("Start_Time");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TotalMarks)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("Total_Marks");

            entity.HasOne(d => d.Course).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quiz_Course");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quiz_Creator");

            entity.HasMany(d => d.Questions).WithMany(p => p.Quizzes)
                .UsingEntity<Dictionary<string, object>>(
                    "QuizQuestion",
                    r => r.HasOne<Question>().WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_QQ_Question"),
                    l => l.HasOne<Quiz>().WithMany()
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_QQ_Quiz"),
                    j =>
                    {
                        j.HasKey("QuizId", "QuestionId").HasName("PK_Quiz_Questions");
                        j.ToTable("QUIZ_QUESTIONS");
                        j.HasIndex(new[] { "QuestionId" }, "IX_QuizQuestions_QuestionID");
                        j.IndexerProperty<int>("QuizId").HasColumnName("QuizID");
                        j.IndexerProperty<int>("QuestionId").HasColumnName("QuestionID");
                    });
        });

        modelBuilder.Entity<QuizAttempt>(entity =>
        {
            entity.HasKey(e => e.AttemptId).HasName("PK__QUIZ_ATT__891A6886181C499B");

            entity.ToTable("QUIZ_ATTEMPT");

            entity.HasIndex(e => e.QuizId, "IX_Attempt_QuizID");

            entity.HasIndex(e => e.UserId, "IX_Attempt_UserID");

            entity.Property(e => e.AttemptId)
                .ValueGeneratedNever()
                .HasColumnName("AttemptID");
            entity.Property(e => e.QuizId).HasColumnName("QuizID");
            entity.Property(e => e.Score).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.StartedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizAttempts)
                .HasForeignKey(d => d.QuizId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attempt_Quiz");

            entity.HasOne(d => d.User).WithMany(p => p.QuizAttempts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attempt_User");
        });

        modelBuilder.Entity<StudentAnswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__STUDENT___D482502499383404");

            entity.ToTable("STUDENT_ANSWER");

            entity.HasIndex(e => new { e.OptionId, e.QuestionId }, "IX_Answer_Option_Question");

            entity.HasIndex(e => e.QuestionId, "IX_Answer_QuestionID");

            entity.HasIndex(e => new { e.AttemptId, e.QuestionId }, "UQ_Answer_Attempt_Question").IsUnique();

            entity.Property(e => e.AnswerId)
                .ValueGeneratedNever()
                .HasColumnName("AnswerID");
            entity.Property(e => e.AttemptId).HasColumnName("AttemptID");
            entity.Property(e => e.MarksAwarded)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("Marks_Awarded");
            entity.Property(e => e.OptionId).HasColumnName("OptionID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

            entity.HasOne(d => d.Attempt).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.AttemptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Attempt");

            entity.HasOne(d => d.Question).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Question");

            entity.HasOne(d => d.Option).WithMany(p => p.StudentAnswers)
                .HasPrincipalKey(p => new { p.OptionId, p.QuestionId })
                .HasForeignKey(d => new { d.OptionId, d.QuestionId })
                .HasConstraintName("FK_Answer_Option_Question");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__USER__1788CCAC18C78DB1");

            entity.ToTable("USER");

            entity.HasIndex(e => e.Email, "UQ__USER__A9D10534FDC79AF4").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserCourseEnrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__USER_COU__7F6877FB7B4DBC54");

            entity.ToTable("USER_COURSE_ENROLLMENT");

            entity.HasIndex(e => e.CourseId, "IX_Enrollment_CourseID");

            entity.HasIndex(e => new { e.UserId, e.CourseId }, "UQ_Enrollment_User_Course").IsUnique();

            entity.Property(e => e.EnrollmentId)
                .ValueGeneratedNever()
                .HasColumnName("EnrollmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Course).WithMany(p => p.UserCourseEnrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Course");

            entity.HasOne(d => d.User).WithMany(p => p.UserCourseEnrollments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_User");
        });

        modelBuilder.Entity<UserCourseTeach>(entity =>
        {
            entity.HasKey(e => e.TeachingId).HasName("PK__USER_COU__69B6BA5EF06EB06C");

            entity.ToTable("USER_COURSE_TEACHES");

            entity.HasIndex(e => e.CourseId, "IX_Teaches_CourseID");

            entity.HasIndex(e => new { e.UserId, e.CourseId }, "UQ_Teaches_User_Course").IsUnique();

            entity.Property(e => e.TeachingId)
                .ValueGeneratedNever()
                .HasColumnName("TeachingID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Course).WithMany(p => p.UserCourseTeaches)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teaches_Course");

            entity.HasOne(d => d.User).WithMany(p => p.UserCourseTeaches)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teaches_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
