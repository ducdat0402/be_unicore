using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class CourseStudentModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseStudent>(entity =>
            {
                entity.ToTable("course_students");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.CourseId).HasColumnName("course_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.StudentClassId).HasColumnName("student_class_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.StartDate).HasColumnName("start_date").IsRequired();
                entity.Property(e => e.EndDate).HasColumnName("end_date").IsRequired();
                entity.Property(e => e.Weight).HasColumnName("weight").HasDefaultValue(1);
                entity.Property(e => e.FinalScore).HasColumnName("final_score").HasColumnType("numeric(5, 2)");
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("ENROLLED");

                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

                entity.HasIndex(e => new { e.CourseId, e.StudentClassId }).IsUnique();

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.CourseStudents)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.StudentClass)
                    .WithMany(sc => sc.CourseStudents)
                    .HasForeignKey(e => e.StudentClassId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
