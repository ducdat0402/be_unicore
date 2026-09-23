using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Service.v1
{
    public class AnnouncementAudienceService : IAnnouncementAudienceService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;

        public AnnouncementAudienceService(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            ISchoolClassRepository schoolClassRepository,
            IStudentClassRepository studentClassRepository,
            ICourseRepository courseRepository,
            ICourseStudentRepository courseStudentRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _schoolClassRepository = schoolClassRepository;
            _studentClassRepository = studentClassRepository;
            _courseRepository = courseRepository;
            _courseStudentRepository = courseStudentRepository;
        }

        public async Task ValidateScopeAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default)
        {
            var scope = scopeType.Trim().ToUpperInvariant();
            if (!AnnouncementConstants.Scope.Supported.Contains(scope))
            {
                throw new InvalidOperationException(
                    "ScopeType must be PUBLIC, STUDENTS, DEPARTMENT, CLASS, COURSE, or SPECIFIC_STUDENTS.");
            }

            var targets = AnnouncementLifecycle.NormalizeIds(targetStudentIds);

            switch (scope)
            {
                case AnnouncementConstants.Scope.Public:
                    if (!string.IsNullOrWhiteSpace(scopeValue) || targets.Count > 0)
                    {
                        throw new InvalidOperationException("PUBLIC scope must not include scopeValue or targetStudentIds.");
                    }
                    break;

                case AnnouncementConstants.Scope.Students:
                    if (!string.IsNullOrWhiteSpace(scopeValue) || targets.Count > 0)
                    {
                        throw new InvalidOperationException("STUDENTS scope must not include scopeValue or targetStudentIds.");
                    }
                    break;

                case AnnouncementConstants.Scope.Department:
                    await EnsureDepartmentAsync(scopeValue, targets, cancellationToken);
                    break;

                case AnnouncementConstants.Scope.Class:
                    await EnsureClassAsync(scopeValue, targets, cancellationToken);
                    break;

                case AnnouncementConstants.Scope.Course:
                    await EnsureCourseAsync(scopeValue, targets, cancellationToken);
                    break;

                case AnnouncementConstants.Scope.SpecificStudents:
                    if (!string.IsNullOrWhiteSpace(scopeValue))
                    {
                        throw new InvalidOperationException("SPECIFIC_STUDENTS scopeValue must be null; provide targetStudentIds.");
                    }
                    if (targets.Count == 0)
                    {
                        throw new InvalidOperationException("SPECIFIC_STUDENTS requires at least one targetStudentId.");
                    }
                    var active = await _userRepository.GetActiveStudentIdsByIdsAsync(targets, cancellationToken);
                    if (active.Count != targets.Count)
                    {
                        var missing = targets.Except(active, StringComparer.OrdinalIgnoreCase);
                        throw new KeyNotFoundException($"Active student(s) not found: {string.Join(", ", missing)}");
                    }
                    break;
            }
        }

        public async Task<IReadOnlyList<string>> ResolveStudentIdsAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default)
        {
            await ValidateScopeAsync(scopeType, scopeValue, targetStudentIds, cancellationToken);
            var scope = scopeType.Trim().ToUpperInvariant();

            return scope switch
            {
                AnnouncementConstants.Scope.Public => Array.Empty<string>(),
                AnnouncementConstants.Scope.Students =>
                    await _userRepository.GetActiveVerifiedStudentIdsAsync(cancellationToken),
                AnnouncementConstants.Scope.Department =>
                    await _studentClassRepository.GetActiveStudentIdsByDepartmentIdAsync(scopeValue!, cancellationToken),
                AnnouncementConstants.Scope.Class =>
                    await _studentClassRepository.GetActiveStudentIdsByClassIdAsync(scopeValue!, cancellationToken),
                AnnouncementConstants.Scope.Course =>
                    await _courseStudentRepository.GetActiveStudentIdsByCourseIdAsync(scopeValue!, cancellationToken),
                AnnouncementConstants.Scope.SpecificStudents =>
                    await _userRepository.GetActiveStudentIdsByIdsAsync(
                        AnnouncementLifecycle.NormalizeIds(targetStudentIds), cancellationToken),
                _ => Array.Empty<string>()
            };
        }

        private async Task EnsureDepartmentAsync(string? scopeValue, List<string> targets, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(scopeValue))
                throw new InvalidOperationException("DEPARTMENT requires scopeValue (department id).");
            if (targets.Count > 0)
                throw new InvalidOperationException("DEPARTMENT must not include targetStudentIds.");

            var dept = await _departmentRepository.GetByIdAsync(scopeValue, ct);
            if (dept == null || dept.IsDeleted || !dept.IsActive)
                throw new KeyNotFoundException($"Active department '{scopeValue}' was not found.");
        }

        private async Task EnsureClassAsync(string? scopeValue, List<string> targets, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(scopeValue))
                throw new InvalidOperationException("CLASS requires scopeValue (class id).");
            if (targets.Count > 0)
                throw new InvalidOperationException("CLASS must not include targetStudentIds.");

            var schoolClass = await _schoolClassRepository.GetByIdAsync(scopeValue, ct);
            if (schoolClass == null || schoolClass.IsDeleted || !schoolClass.IsActive)
                throw new KeyNotFoundException($"Active class '{scopeValue}' was not found.");
        }

        private async Task EnsureCourseAsync(string? scopeValue, List<string> targets, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(scopeValue))
                throw new InvalidOperationException("COURSE requires scopeValue (course id).");
            if (targets.Count > 0)
                throw new InvalidOperationException("COURSE must not include targetStudentIds.");

            var course = await _courseRepository.GetByIdAsync(scopeValue, ct);
            if (course == null || course.IsDeleted || !course.IsActive)
                throw new KeyNotFoundException($"Active course '{scopeValue}' was not found.");
        }
    }
}
