using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.Service.v1;
using UniCore.Helper.Constant;

namespace UniCore.Application.Service.v1
{
    public class AnnouncementAudienceService : IAnnouncementAudienceService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IStudentClassRepository _studentClassRepository;

        public AnnouncementAudienceService(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            ISchoolClassRepository schoolClassRepository,
            IStudentClassRepository studentClassRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _schoolClassRepository = schoolClassRepository;
            _studentClassRepository = studentClassRepository;
        }

        public async Task ValidateScopeAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default)
        {
            var normalizedScope = scopeType.Trim().ToUpperInvariant();

            if (!AnnouncementConstants.Scope.Supported.Contains(normalizedScope))
            {
                throw new InvalidOperationException(
                    $"Scope '{scopeType}' is not supported. Use PUBLIC, DEPARTMENT, CLASS, or STUDENT.");
            }

            var targets = NormalizeIds(targetStudentIds);

            switch (normalizedScope)
            {
                case AnnouncementConstants.Scope.Public:
                    if (!string.IsNullOrWhiteSpace(scopeValue))
                    {
                        throw new InvalidOperationException("scopeValue must be null when scopeType is PUBLIC.");
                    }
                    if (targets.Count > 0)
                    {
                        throw new InvalidOperationException("targetStudentIds are not allowed when scopeType is PUBLIC.");
                    }
                    break;

                case AnnouncementConstants.Scope.Department:
                    if (string.IsNullOrWhiteSpace(scopeValue))
                    {
                        throw new InvalidOperationException("scopeValue (department id) is required when scopeType is DEPARTMENT.");
                    }
                    if (targets.Count > 0)
                    {
                        throw new InvalidOperationException("targetStudentIds are not allowed when scopeType is DEPARTMENT.");
                    }

                    var department = await _departmentRepository.GetByIdAsync(scopeValue, cancellationToken);
                    if (department == null || department.IsDeleted || !department.IsActive)
                    {
                        throw new KeyNotFoundException($"Active department with ID '{scopeValue}' was not found.");
                    }
                    break;

                case AnnouncementConstants.Scope.Class:
                    if (string.IsNullOrWhiteSpace(scopeValue))
                    {
                        throw new InvalidOperationException("scopeValue (class id) is required when scopeType is CLASS.");
                    }
                    if (targets.Count > 0)
                    {
                        throw new InvalidOperationException("targetStudentIds are not allowed when scopeType is CLASS.");
                    }

                    var schoolClass = await _schoolClassRepository.GetByIdAsync(scopeValue, cancellationToken);
                    if (schoolClass == null || schoolClass.IsDeleted || !schoolClass.IsActive)
                    {
                        throw new KeyNotFoundException($"Active class with ID '{scopeValue}' was not found.");
                    }
                    break;

                case AnnouncementConstants.Scope.Student:
                    if (!string.IsNullOrWhiteSpace(scopeValue))
                    {
                        throw new InvalidOperationException(
                            "scopeValue must be null when scopeType is STUDENT. Provide targetStudentIds (stored in announcement_students).");
                    }
                    if (targets.Count == 0)
                    {
                        throw new InvalidOperationException(
                            "At least one targetStudentId is required when scopeType is STUDENT.");
                    }

                    var activeStudents = await _userRepository.GetActiveStudentIdsByIdsAsync(targets, cancellationToken);
                    if (activeStudents.Count != targets.Count)
                    {
                        var missing = targets.Except(activeStudents, StringComparer.OrdinalIgnoreCase);
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

            var normalizedScope = scopeType.Trim().ToUpperInvariant();

            return normalizedScope switch
            {
                AnnouncementConstants.Scope.Public =>
                    Array.Empty<string>(),

                AnnouncementConstants.Scope.Department =>
                    await _studentClassRepository.GetActiveStudentIdsByDepartmentIdAsync(scopeValue!, cancellationToken),

                AnnouncementConstants.Scope.Class =>
                    await _studentClassRepository.GetActiveStudentIdsByClassIdAsync(scopeValue!, cancellationToken),

                AnnouncementConstants.Scope.Student =>
                    await _userRepository.GetActiveStudentIdsByIdsAsync(NormalizeIds(targetStudentIds), cancellationToken),

                _ => Array.Empty<string>()
            };
        }

        private static List<string> NormalizeIds(IEnumerable<string>? ids)
        {
            if (ids == null)
            {
                return new List<string>();
            }

            return ids
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
