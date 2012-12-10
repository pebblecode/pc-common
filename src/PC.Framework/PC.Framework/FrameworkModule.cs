using PebbleCode.Framework.Email;
using PebbleCode.Framework.IoC;

namespace PebbleCode.Framework
{
    public class FrameworkModule : BaseNinjectModule
    {
        public override void Load()
        {
            Bind<IEmailer>().To<Emailer>();
        }
    }
}
