using Data.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void RegisterEntityTypeConfiguration(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        var applyGenericMethod = typeof(ModelBuilder).GetMethods().First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

        var types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic);

        foreach (var type in types)
        {
            foreach (var iface in type.GetInterfaces())
            {
                if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                {
                    var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                    applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type) });
                }
            }
        }
    }

    public static void RegisterAllEntities(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(IBaseEntity).IsAssignableFrom(c));

        foreach (var type in types)
        {
            modelBuilder.Entity(type);
        }
    }

    public static List<T> ExecuteSqlRaw<T>(this DatabaseFacade db, string query, Func<DbDataReader, T> map)
    {
        using var command = db.GetDbConnection().CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;

        db.OpenConnection();
        using var result = command.ExecuteReader();
        var entities = new List<T>();
        while (result.Read())
        {
            entities.Add(map(result));
        }

        return entities;
    }
}