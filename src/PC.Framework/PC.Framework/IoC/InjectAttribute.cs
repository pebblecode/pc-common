using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.IoC
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
    }
}
