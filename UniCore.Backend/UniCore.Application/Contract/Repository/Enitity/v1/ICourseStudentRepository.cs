using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface ICourseStudentRepository : IRepository<CourseStudent>
    {
        Task<List<string>> GetActiveStudentIdsByCourseIdAsync(string courseId, CancellationToken cancellationToken = default);
    }
}
