using System.Linq;
using System.Reflection;
using PebbleCode.Framework;
using PebbleCode.Framework.IoC;
using PebbleCode.Repository;

namespace PebbleCode.Entities.Tests.Integration.Repository
{

    /// <summary>
    /// Sets up Ninject bindings for mocking out all database repositories
    /// </summary>
    /// 
    
  public class RepositoryNinjectModule : BaseNinjectModule
    {
      public override void Load()
        {
            //Mock each of the repositories
            Assembly repos = Assembly.Load("PC.Entities.Tests.Integration");
            repos.GetTypes()
                .Where(type => typeof (EntityRepository).IsAssignableFrom(type) && !type.IsAbstract)
                .ForEach(repoType => Bind(repoType).ToMockSingleton());
        }
    }
}
