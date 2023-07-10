using AutoMapper;
using Google.Apis.Upload;
using Microsoft.EntityFrameworkCore;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Domain.Entities;
using MRE.Presistence.Abstruct;
using MRE.Presistence.Context;

namespace MRE.Presistence.Concrete
{
    public class ProjectRpository : IProjectRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public ProjectRpository(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Project Create(ProjectModel model)
        {
            var projectEntity = _mapper.Map<ProjectModel, Project>(model);
            _db.Projects.Add(projectEntity);
            _db.SaveChanges();
            return projectEntity;
        }

        public Project Update(ProjectModel model)
        {

            var projectDb = FindById(model.Id);
            if (projectDb != null)
            {
                _mapper.Map<ProjectModel, Project>(model, projectDb);
                _db.Update(projectDb);
                _db.SaveChanges();
            }
            else
            {
                return null;
            }

            _db.SaveChanges();
            return projectDb;
        }

        public void Delete(Guid id)
        {
            var projectDb = FindById(id);

            if (projectDb is null)
            {
                throw new Exception("The record you are trying to delete does not exist.");
            }

            _db.Projects.Remove(projectDb);
            _db.SaveChanges();
        }

        private Project FindById(Guid userId)
        {
            return _db.Projects.FirstOrDefaultAsync(a => a.Id == userId)?.Result;
        }

        public ProjectDto GetById(Guid id)
        {
            var project = FindById(id);
            if (project is null)
            {
                return null;
            }
            return _mapper.Map<Project, ProjectDto>(project);
        }

        public List<ProjectDto> List()
        {
            var projects = _db.Projects.AsNoTracking().ToList();

            if (projects is null)
            {
                return new List<ProjectDto>();
            }

            return _mapper.Map<List<Project>, List<ProjectDto>>(projects);
        }
    }
}
