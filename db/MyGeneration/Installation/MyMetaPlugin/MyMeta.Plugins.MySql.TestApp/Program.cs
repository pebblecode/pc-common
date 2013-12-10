using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MyMeta.Plugins.MySql.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MyMetaPluginContext context = new MyMetaPluginContext("RedSkyMySql 1.0", "Database=fb_dev;Data Source=localhost;Port=3400;User Id=gambler;Password=g@mbl3r;");
            IMyMetaPlugin plugin = new RedSkyMySqlPlugin();
            plugin.Initialize(context);
            DataTable databases = plugin.Databases;
            DataTable procedures = plugin.GetProcedures("fb_dev");
            DataTable parameters = plugin.GetProcedureParameters("fb_dev", "sp_engine_get_tickets");

        }
    }
}
