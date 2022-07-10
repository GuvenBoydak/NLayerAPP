using Autofac;
using NLayer.Caching;
using NLayer.Core;
using NLayer.Repository;
using NLayer.Service;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.API.Modules
{
    public class RepoServiceModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();


            Assembly apiAssembly = Assembly.GetExecutingAssembly();

            Assembly repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));

            Assembly serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            //apiAssembly, repoAssembly, serviceAssembly git bunlarda ara x=>.x.Name'i "Repository" ile bitenleri al ve bunlarında Interfacelerinide implemente et diyoruz.InstancePerLifetimeScope ise => Asp.Net Core daki AddScope a karşılık gelıyor.
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //apiAssembly, repoAssembly, serviceAssembly git bunlarda ara x=>.x.Name'i "Service" ile bitenleri al ve bunlarında Interfacelerinide implemente et diyoruz.InstancePerLifetimeScope ise => Asp.Net Core daki AddScope a karşılık gelıyor.
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();
            //builder.RegisterType<CategoryServiceWithCaching>().As<ICategoryService>();


        }

    }
}
