using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PebbleCode.Framework.Logging
{
   public interface ILogger
    {
       void WriteInfo(string log, string category);
       void WriteUnexpectedException(Exception ex, string title, string category);
       void WriteWarning(string warning, string category);
    }
}

