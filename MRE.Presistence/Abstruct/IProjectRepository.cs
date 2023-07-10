using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Domain.Entities;

namespace MRE.Presistence.Abstruct
{
    public interface IProjectRepository
    {
        Project Create(ProjectModel model);
        Project Update(ProjectModel model);
        void Delete(Guid userId);
        ProjectDto GetById(Guid id);
        List<ProjectDto> List();
    }
}
