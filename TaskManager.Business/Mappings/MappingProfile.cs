using AutoMapper;
using TaskManager.Business.DTOs.Auth;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TaskItem to TaskDTO
        CreateMap<TaskItem, TaskDTO>();

        CreateMap<CreateTaskDTO, TaskItem>();

        CreateMap<User, AuthResponseDTO>();

    }
}
