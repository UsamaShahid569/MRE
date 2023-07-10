using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MRE.Contracts.Dtos;
using MRE.Contracts.Enums;
using MRE.Contracts.Models;
using MRE.Domain.Entities.Identity;
using MRE.Presistence.Abstruct;
using MRE.Presistence.Concrete.Base;
using MRE.Presistence.Context;
using MRE.Presistence.Extensions;
using MRE.Presistence.Helpers;
using MRE.Presistence.IProvider;
using Microsoft.AspNetCore.Hosting;


namespace MRE.Presistence.Concrete
{
    public class UserRepository : ServiceBaseEntity<User>, IUserRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        private readonly ICurrentUserProvider _currentUserProvider; 
        

        public UserRepository(DataContext db,
            ICurrentUserProvider currentUserProvider,IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
            _currentUserProvider = currentUserProvider;
        }
        public IQueryable<User> Query()
        {
            return GetAllWithoutQueryFilter().AsNoTracking();
        }
        public UserModel Get(Guid userId)
        {
            var user = FindById(userId);
            var role = _db.UserRoles.FirstOrDefault(x => x.UserId == userId);
            

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                RoleId = role != null ? role.RoleId:null,
            };
        }

        public void AddLoginLog(UserLogin model)
        {
            _db.UserLogins.Add(model);
            _db.SaveChanges();
            
        }

        public UserProfileDto GetMemberProfile()
        {
            return  _db.Users.ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider).FirstOrDefault(a => a.Id == _currentUserProvider.UserId);
            
        }
        public void UpdateProfile(UserProfileModel model)
        {
            var user = _db.Users
                .FirstOrDefault(a => a.Id == _currentUserProvider.UserId);
            if (user != null)
            {
                user.FullName = model.FullName;
                
                _db.SaveChanges();
            }
        }

        public int Count(string search)
        {
            IQueryable<User> query = GetAll().Include(x => x.UserRoles).ThenInclude((UserRole x) => x.Role);
            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FullName.Contains(search) || x.Email.Contains(search));
            }

            return query.Count();
        }
        public DataAndCountDto<UserDto> Get(int? skip, int? take, string search, string orderBy, Guid? roleId)
        {
            IQueryable<User> query = GetAll().Include(x => x.UserRoles).ThenInclude((UserRole x) => x.Role);
            if (roleId.HasValue)
            {
                query = query.Where(a => a.UserRoles.Any(b => b.RoleId == roleId));
            }
            
            

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x =>  x.FullName.Contains(search) || x.Email.Contains(search) );
            }

            if (!String.IsNullOrEmpty(orderBy))
            {
                var propertyName = orderBy.Split("-")[0];
                var isReverse = orderBy.Split("-")[1] == "desc" ? true : false;

                query = isReverse ? query.OrderByDescendingEx(propertyName) : query.OrderByEx(propertyName);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            var user = new DataAndCountDto<UserDto>();
            user.Count = query.Count();
            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            user.Data = query.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToList();


         

            return user;
        }
        
        public User Create(UserModel model)
        {

            var user = new User
            {
                Email = model.Email,
                FullName = model.FullName,
                TenantId = _currentUserProvider.TenantId,
            };

            

            var userObj = _db
                .GetAllWithoutQueryFilter<User>().FirstOrDefault(x => x.Email == user.Email && x.TenantId == user.TenantId);

            if (userObj == null)
            {
                if (model.RoleId.HasValue)
                {
                    user.UserRoles = new List<UserRole>()
                    {
                        new UserRole
                                {
                                    RoleId = model.RoleId.Value
                                }
                    };
                }
                _db.Users.Add(user);
                
               
                _db.SaveChanges();
                
            }
            else
            {
                if (userObj.ValidUntil != null)
                {
                    userObj.ValidUntil = null;
                    _db.SaveChanges();

                    this.Update(new UserModel
                    {
                        Id = userObj.Id,
                        FullName = model.FullName,
                        RoleId = model.RoleId,
                    });

                }
            }

            return user;
        }

        public User Update(UserModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    var user = _db.Users.FirstOrDefaultAsync(a=>a.Id == model.Id).Result;
                    user.FullName = model.FullName;
                    

                    
                        var oldUserRole = _db.UserRoles.FirstOrDefault(x => x.UserId == model.Id);
                    
                        if (oldUserRole != null && model.RoleId.HasValue)
                        {
                            oldUserRole.RoleId = model.RoleId.Value;
                    }
                    else
                    {
                        if (model.RoleId.HasValue)
                        {
                            user.UserRoles = new List<UserRole>()
                            {
                                new UserRole
                            {
                                RoleId = model.RoleId.Value
                            }
                            };
                           
                        }
                    }

                        _db.SaveChanges();

                    transaction.Commit();
                    return user;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

      

        public void Delete(Guid userId)
        {
            if (_currentUserProvider.UserId == userId)
            {
                throw new Exception("Cannot delete the user logged in.");
            }
            var user = FindById(userId);
            user.ValidUntil = DateTime.Now;
            _db.SaveChanges();
        }
        public void UpdateStatus(Guid id, bool status)
        {
            var obj = _db.Users.FirstOrDefault(a => a.Id == id);
            if (obj != null)
            {
                obj.IsActive = status;
                _db.SaveChanges();
            }
        }
        public bool ChangeStatus(Guid userId, bool IsActive)
        {
            if (_currentUserProvider.UserId == userId)
            {
                throw new Exception("Cannot change the user logged in.");
            }
            var user = FindById(userId);
            user.IsActive = IsActive;
            _db.SaveChanges();
            return user.IsActive.Value;
        }

        
        

    }
}
