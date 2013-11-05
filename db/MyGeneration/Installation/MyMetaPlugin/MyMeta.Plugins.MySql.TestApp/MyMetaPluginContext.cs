using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MyMeta.Plugins.MySql.TestApp
{
    class MyMetaPluginContext__ : IMyMetaPluginContext
    {
        #region IMyMetaPluginContext Members

        public string ConnectionString
        {
            get { return "Database=fb_v01;Data Source=localhost;User Id=root;Password=macca7;"; }
        }

        public System.Data.DataTable CreateColumnsDataTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateDatabasesDataTable()
        {
            DataTable metaData = new DataTable();
            metaData.Columns.Add(new DataColumn("CATALOG_NAME"));
            metaData.Columns.Add(new DataColumn("DESCRIPTION"));
            return metaData;
        }

        public System.Data.DataTable CreateDomainsDataTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateForeignKeysDataTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateIndexesDataTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateParametersDataTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateProceduresDataTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateResultColumnsDataTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateTablesDataTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateViewsDataTable()
        {
            throw new NotImplementedException();
        }

        public bool IncludeSystemEntities
        {
            get { throw new NotImplementedException(); }
        }

        public string ProviderName
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
