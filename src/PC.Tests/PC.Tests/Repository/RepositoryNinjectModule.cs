using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Ninject;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;

using PebbleCode.Framework;
using PebbleCode.Framework.IoC;
using PebbleCode.Repository;

namespace PebbleCode.Tests.Repository
{

    /// <summary>
    /// Sets up Ninject bindings for mocking out all database repositories
    /// </summary>
  public class RepositoryNinjectModule : BaseNinjectModule
    {
      public override void Load()
        {
            //Mock each of the repositories
            Assembly repos = Assembly.Load("PC.Tests");
            repos.GetTypes()
                .Where(type => typeof (EntityRepository).IsAssignableFrom(type) && !type.IsAbstract)
                .ForEach(repoType => Bind(repoType).ToMockSingleton());
        }
    }
}
