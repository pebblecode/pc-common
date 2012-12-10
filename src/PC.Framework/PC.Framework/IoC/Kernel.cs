using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using Ninject.Activation;
using System.IO;
using System.Reflection;
using Ninject.Parameters;

namespace PebbleCode.Framework.IoC
{
    /// <summary>
    /// A container for the dependency injection container... META
    /// </summary>
    public static class Kernel
    {
        private static IKernel _kernel = null;

        /// <summary>
        /// Initialise the kernel, load any assemblies which define bindings
        /// </summary>
        static Kernel()
        {
            NinjectSettings settings = new NinjectSettings();
            settings.InjectAttribute = typeof(InjectAttribute);

            _kernel = new StandardKernel(settings);

            // Load additionally configured assemblies
            if (IoCKernelConfiguration.Instance != null)
            {
                foreach (BindingAssembly bindingAssembly in IoCKernelConfiguration.Instance.BindingAssemblys)
                {
                    try
                    {
                        var assembly = Assembly.Load(bindingAssembly.Assembly);
                        _kernel.Load(assembly);
                    }
                    catch (FileNotFoundException)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to load IoC assembly " + bindingAssembly.Assembly);
                    }
                }
            }
        }

        /// <summary>
        /// Access to the gloabally configured kernel
        /// </summary>
        public static IKernel Instance
        {
            get
            {
                return _kernel;
            }
            set
            {
                _kernel = value;
            }
        }

        /// <summary>
        /// Shortcut for Instance.Get T, removing the need to reference the dependency injection assembly in the caller
        /// </summary>
        /// <typeparam name="T">The contract type to resolve</typeparam>
        /// <returns></returns>
        public static T Get<T>(params IParameter[] parameters)
        {
            return Instance.Get<T>(parameters);
        }

        public static object Get(Type type)
        {
            return Instance.Get(type);
        }

        /// <summary>
        /// Injects dependencies into an objects without managing the object lifecycle
        /// </summary>
        /// <param name="o"></param>
        public static void Inject(object o)
        {
            Instance.Inject(o);
        }
    }
}
