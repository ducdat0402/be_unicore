using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class StudentClassRepository : RepositoryEFCoreBase<StudentClass>, IStudentClassRepository
    {
        public StudentClassRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<string>> GetActiveStudentIdsByClassIdAsync(string classId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(sc =>
                    sc.ClassId == classId &&
                    sc.IsActive &&
                    !sc.IsDeleted &&
                    sc.Status == "ACTIVE" &&
                    sc.Student.IsActive)
                .Select(sc => sc.StudentId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetActiveStudentIdsByDepartmentIdAsync(string departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(sc =>
                    sc.SchoolClass.DepartmentId == departmentId &&
                    sc.SchoolClass.IsActive &&
                    !sc.SchoolClass.IsDeleted &&
                    sc.IsActive &&
                    !sc.IsDeleted &&
                    sc.Status == "ACTIVE" &&
                    sc.Student.IsActive)
                .Select(sc => sc.StudentId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
