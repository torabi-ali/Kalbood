using System.Reflection;
using Data.Domain.Common;
using Data.Extensions;
using Data.Utility;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DbContext;

public class KalboodDbContext(DbContextOptions<KalboodDbContext> options) : IdentityDbContext(options), IDataProtectionKeyContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var entitiesAssembly = typeof(IBaseEntity).Assembly;

        builder.RegisterAllEntities(entitiesAssembly);
        builder.RegisterEntityTypeConfiguration(entitiesAssembly);
    }

    #region DbSet

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }


    #endregion

    #region Overide

    public override int SaveChanges()
    {
        var entities = ChangeTracker.Entries()
            .Where(p => p.State is EntityState.Added or EntityState.Modified)
            .Select(p => p.Entity)
            .ToArray();

        foreach (var entity in entities)
        {
            CleanStringEntity(entity);
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries()
            .Where(p => p.State is EntityState.Added or EntityState.Modified)
            .Select(p => p.Entity)
            .ToArray();

        foreach (var entity in entities)
        {
            CleanStringEntity(entity);
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    #endregion Overide

    #region Methods

    private static void CleanStringEntity<TEntity>(TEntity entity)
    {
        var propertyInfos = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

        foreach (var propertyInfo in propertyInfos)
        {
            var val = (string)propertyInfo.GetValue(entity, null);
            if (!string.IsNullOrWhiteSpace(val))
            {
                var newVal = val.CleanString();
                if (newVal == val)
                {
                    continue;
                }

                propertyInfo.SetValue(entity, newVal, null);
            }
        }
    }

    #endregion
}
