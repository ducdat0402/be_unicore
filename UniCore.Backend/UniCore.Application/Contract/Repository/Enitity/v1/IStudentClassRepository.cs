using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IStudentClassRepository : IRepository<StudentClass>
    {
        Task<List<string>> GetActiveStudentIdsByClassIdAsync(string classId, CancellationToken cancellationToken = default);
        Task<List<string>> GetActiveStudentIdsByDepartmentIdAsync(string departmentId, CancellationToken cancellationToken = default);
    }
}
