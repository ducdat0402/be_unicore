using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class CourseStudentRepository : RepositoryEFCoreBase<CourseStudent>, ICourseStudentRepository
    {
        public CourseStudentRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<string>> GetActiveStudentIdsByCourseIdAsync(string courseId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(cs =>
                    cs.CourseId == courseId &&
                    cs.IsActive &&
                    !cs.IsDeleted &&
                    cs.StudentClass.IsActive &&
                    !cs.StudentClass.IsDeleted &&
                    cs.StudentClass.Status == "ACTIVE" &&
                    cs.StudentClass.Student.IsActive)
                .Select(cs => cs.StudentClass.StudentId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
