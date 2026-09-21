using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class AnnouncementStudentModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnouncementStudent>(entity =>
            {
                entity.ToTable("announcement_students");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
                entity.Property(e => e.AnnouncementId).HasColumnName("announcement_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.StudentId).HasColumnName("student_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.ViewedAt).HasColumnName("viewed_at");
                entity.Property(e => e.AcknowledgedAt).HasColumnName("acknowledged_at");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("sysutcdatetime()");

                entity.HasIndex(e => new { e.AnnouncementId, e.StudentId }).IsUnique();
                entity.HasIndex(e => e.StudentId);

                entity.HasOne(e => e.Announcement)
                    .WithMany(a => a.AnnouncementStudents)
                    .HasForeignKey(e => e.AnnouncementId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Student)
                    .WithMany(u => u.AnnouncementStudents)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
