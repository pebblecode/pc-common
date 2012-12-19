using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Activation;
using Ninject.MockingKernel;
using Ninject.Syntax;

namespace PebbleCode.Framework.IoC
{
   public static class NinjectMoqExtensions
    {
       public static IBindingWhenInNamedWithOrOnSyntax<T> ToMock<T>(this IBindingToSyntax<T> binding)
        {
            return binding.ToMethod(CreateMockObject<T>);
        }

        private static T CreateMockObject<T>(IContext ctx)
        {
            IMockProviderCallbackProvider callBackProvider =
                    ctx.Kernel.Components.Get<IMockProviderCallbackProvider>();
            IProvider factory = callBackProvider.GetCreationCallback().Invoke(ctx);
            return (T)factory.Create(ctx);
        }

        public static IBindingNamedWithOrOnSyntax<T> ToMockSingleton<T>(this IBindingToSyntax<T> binding)
        {
            return binding.ToMock().InSingletonScope();
        }
    }
}
