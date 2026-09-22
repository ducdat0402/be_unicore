using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;

namespace UniCore.Application.MapperProfile
{
    public class AnnouncementMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Announcement, AnnouncementDTO>()
                .Map(
                    dest => dest.TargetStudentIds,
                    src => src.AnnouncementStudents == null
                        ? new List<string>()
                        : src.AnnouncementStudents.Select(s => s.StudentId).ToList());
        }
    }
}
