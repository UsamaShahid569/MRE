using AutoMapper;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Domain.Entities;

namespace MRE.Profiles
{
    public class ProjectAutoMapperProfile : Profile
    {
        public ProjectAutoMapperProfile() 
        {
            CreateMap<ProjectDto, Project>().ReverseMap();
            CreateMap<ProjectModel, Project>().ReverseMap();
            CreateMap<ProjectModel, ProjectDto>().ReverseMap();

        }
    }
}
