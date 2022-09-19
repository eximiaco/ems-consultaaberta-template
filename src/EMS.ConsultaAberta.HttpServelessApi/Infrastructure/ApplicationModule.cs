using System.Reflection;
using Autofac;
using EMS.ConsultaAberta.Crosscutting;
using EMS.ConsultaAberta.SeedWork;

namespace EMS.ConsultaAberta.HttpServelessApi.Infrastructure;

public class ApplicationModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var domainAssembly = typeof(DatabaseOptions).GetTypeInfo().Assembly;
        
        builder
            .RegisterAssemblyTypes(domainAssembly)
            .AsClosedTypesOf(typeof(IQueryService<>))
            .InstancePerLifetimeScope();
        
        builder
            .RegisterAssemblyTypes(domainAssembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();
    }
}