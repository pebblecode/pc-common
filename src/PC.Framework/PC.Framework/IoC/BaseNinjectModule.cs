using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Ninject.Activation;

namespace PebbleCode.Framework.IoC
{
    /// <summary>
    /// A base class with utility functions to assist with binding
    /// </summary>
    public class BaseNinjectModule : NinjectModule
    {
        public override void Load()
        {
        }
        protected bool TargetHasAttributes<T1, T2>(IRequest request)
            where T1 : Attribute
            where T2 : Attribute
        {
            return request.Target.Type.Defines<T1>() && request.Target.Type.Defines<T2>();
        }

        protected bool TargetHasAttributes<T1, T2, T3>(IRequest request)
            where T1 : Attribute
            where T2 : Attribute
            where T3 : Attribute
        {
            return request.Target.Type.Defines<T1>() && request.Target.Type.Defines<T2>() && request.Target.Type.Defines<T3>();
        }
    }
}
