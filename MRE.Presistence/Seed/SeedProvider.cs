using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRE.Contracts.Enums;
using MRE.Contracts.Models;
using MRE.Domain.Entities;
using MRE.Domain.Entities.Identity;
using MRE.Domain.Enums;
using MRE.Presistence.Context;
using MRE.Presistence.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Model.Models;

namespace MRE.Presistence.Seed
{
   public class SeedProvider : ISeedProvider
    {
        private readonly DataContext _db;
        private readonly IOptions<List<SuperAdminUserModel>> _dbOptions;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SeedProvider(DataContext db, IOptions<List<SuperAdminUserModel>> dbOptions,
            IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _dbOptions = dbOptions;
            _hostingEnvironment = hostingEnvironment;
        }

        public void InitDevelopment()
        {
            _db.Database.EnsureCreated();
            SeedRoles();
            SeedSuperAdminUsers();
            SeedLookups();
        }

        public void InitProduction()
        {
            _db.Database.EnsureCreated();
            SeedRoles();
            SeedSuperAdminUsers();
            SeedLookups();
        }

        private void SeedSuperAdminUsers()
        {
            if (!_db.GetAllWithoutQueryFilter<User>().Any())
            {
                _dbOptions.Value.ForEach(model =>
                {
                    var user = new User
                    {
                        Email = model.Email,
                        FullName = model.FullName,
                        IsActive = true
                    };

                    var result = _db.Users.Add(user);
                    _db.SaveChanges();
                    var role = _db.Roles.FirstOrDefault(a => a.Name == RolesEnum.SuperAdmin);
                    if (role!= null)
                    {
                        _db.UserRoles.Add(new UserRole
                        {
                            UserId = user.Id,
                            RoleId = role.Id
                        });
                        _db.SaveChanges();
                    }

                });
            }
        }

      

        private void SeedRoles()
        {
            if (!_db.Roles.Any())
            {
                _db.Roles.AddRange(
                     new Role() { Name = RolesEnum.SuperAdmin },
                     new Role { Name = RolesEnum.Admin }
                     );

                _db.SaveChanges();
            }
            
        }
       
        private void SeedLookups()
        {
            //Seed Data
        }
    }

}
