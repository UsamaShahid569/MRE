using MRE.Domain.Entities;
using MRE.Domain.Entities.Base;
using MRE.Domain.Entities.Identity;
using MRE.Presistence.IProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using MRE.Domain.Entities.Business;

namespace MRE.Presistence.Context
{
    public class DataContext : DbContext
    {
        public bool? ShowArchive = false;

        private readonly ICurrentUserProvider _currentUserProvider;

        public DataContext(DbContextOptions<DataContext> options, ICurrentUserProvider currentUserProvider) : base(options)
        {
            _currentUserProvider = currentUserProvider;
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }

        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<LookupParent> LookupParents { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<BusinessGroup> BusinessGroups { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactEmail> ContactEmails { get; set; }
        public DbSet<ContactPhone> ContactPhones { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            DisableCacadeDelete(builder);
            SetCacadeDelete(builder);
            EnableQueryFilters(builder);
            SetDefaultValues(builder);

            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<UserLogin>(entity =>
            {
                entity.HasOne(ur => ur.User)
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired(false);

            });

        }

        public override int SaveChanges()
        {
            try
            {
                var now = DateTime.Now;

                var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList();

                foreach (var entry in entries)
                {
                    if (entry.Entity is IDomainObject)
                    {
                        var domainEntity = (IDomainObject)entry.Entity;

                        if (entry.State == EntityState.Added)
                        {
                            domainEntity.CreatedDate = domainEntity.CreatedDate.HasValue ? domainEntity.CreatedDate : now;
                            domainEntity.IsActive = true;
                            if (domainEntity.TenantId == null)
                            {
                                domainEntity.TenantId = _currentUserProvider.TenantId;
                            }

                            if (domainEntity.CreatedBy == null)
                            {
                                domainEntity.CreatedBy = _currentUserProvider.UserId;
                            }

                            this.SetIdentityValues(entry);
                            this.SetDefaultValues(entry);
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            domainEntity.UpdatedDate = domainEntity.UpdatedDate.HasValue ? domainEntity.UpdatedDate : now;

                            if (domainEntity.UpdatedBy == null)
                            {
                                domainEntity.UpdatedBy = _currentUserProvider.UserId;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base.SaveChanges();
        }

        public IQueryable<User> GetUserWithTenantId()
        {
            return this.Users.Where(x => x.TenantId == _currentUserProvider.TenantId && x.ValidUntil == null);
        }

        public IQueryable<T> GetAll<T>() where T : class, IDomainObject
        {
            IQueryable<T> set;

            switch (ShowArchive)
            {
                case false:
                    set = Set<T>();
                    break;

                case true:
                    set = Set<T>().IgnoreQueryFilters().Where(x => x.ValidUntil != null);
                    break;

                case null:
                    set = Set<T>().IgnoreQueryFilters();
                    break;

                default:
                    throw new NotSupportedException();
            }

            if (_currentUserProvider.IsSuperAdmin == false)
            {
                set = set.Where(x => x.TenantId == _currentUserProvider.TenantId);
            }
            else
            {
                set = Set<T>().IgnoreQueryFilters();

                if (ShowArchive == false)
                {
                    set = set.Where(x => x.ValidUntil == null);
                }
            }

            return set;
        }

        public IQueryable<T> GetAllWithoutQueryFilter<T>() where T : class, IDomainObject
        {
            return Set<T>().IgnoreQueryFilters();
        }

        private void DisableCacadeDelete(ModelBuilder builder)
        {
            var cascadeFKs = builder.Model.GetEntityTypes()
             .SelectMany(t => t.GetForeignKeys())
             .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private void SetCacadeDelete(ModelBuilder builder)
        {


            builder.Entity<LookupParent>().HasMany(x => x.Lookups)
               .WithOne(x => x.LookupParent).HasForeignKey(x => x.LookupParentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void SetDefaultValues(ModelBuilder builder)
        {
            builder.Entity<User>().Property(x => x.IsActive).HasDefaultValue(true);
        }

        private void SetIdentityValues(EntityEntry entry)
        {

        }

        private void SetDefaultValues(EntityEntry entry)
        {

        }

        #region Global Query For Multi Tenant

        private void EnableQueryFilters(ModelBuilder builder)
        {
            foreach (var type in GetEntityTypes())
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { builder });
            }
            builder.Entity<User>().HasQueryFilter(x => x.ValidUntil == null);
        }

        static readonly MethodInfo SetGlobalQueryMethod = typeof(DataContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                        .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : DomainObject
        {
            builder.Entity<T>().HasQueryFilter(e => e.TenantId == _currentUserProvider.TenantId && e.ValidUntil == null);
        }

        private static IList<Type> _entityTypeCache;
        private static IList<Type> GetEntityTypes()
        {
            if (_entityTypeCache != null)
            {
                return _entityTypeCache.ToList();
            }

            var baseEntityType = typeof(DomainObject);
            var types = baseEntityType.Assembly.GetTypes();
            _entityTypeCache = types.Where(x => !x.IsAbstract && baseEntityType.IsAssignableFrom(x))
                .ToList();

            return _entityTypeCache;
        }

        #endregion

    }
}
