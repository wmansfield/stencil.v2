using System;
using Microsoft.EntityFrameworkCore;
using Placeholder.Common;
using Placeholder.Common.Configuration;
using Placeholder.Data.Sql.Models;
using Zero.Foundation;
using Zero.Foundation.Aspect;

namespace Placeholder.Data.Sql
{
    public class PlaceholderContextFactory : ChokeableClass, IPlaceholderContextFactory
    {
        public PlaceholderContextFactory(IFoundation foundation)
            : base(foundation)
        {
            this.ConnectionStringCache = new AspectCache("PlaceholderContextFactory", this.IFoundation);
        }

        protected AspectCache ConnectionStringCache { get; set; }


        public virtual PlaceholderContext CreateSharedContext()
        {
            return base.ExecuteFunction<PlaceholderContext>("CreateSharedContext", delegate ()
            {
                DbContextOptionsBuilder<PlaceholderContext> builder = new DbContextOptionsBuilder<PlaceholderContext>()
                    .UseSqlServer<PlaceholderContext>(
                        this.GetSqlConnectionString(CommonAssumptions.SQL_PRIMARY_CODE), 
                        options => options.EnableRetryOnFailure());

                return new PlaceholderContext(builder.Options);
            });
        }

        public virtual PlaceholderContext CreateIsolatedContext(string code)
        {
            return base.ExecuteFunction<PlaceholderContext>("CreateIsolatedContext", delegate ()
            {
                 DbContextOptionsBuilder<PlaceholderContext> builder = new DbContextOptionsBuilder<PlaceholderContext>()
                    .UseSqlServer(
                        this.GetSqlConnectionString(code),
                        options => options.EnableRetryOnFailure());

                return new PlaceholderContext(builder.Options);
            });
        }

        protected virtual string GetSqlConnectionString(string code)
        {
            return base.ExecuteFunction<string>("GetSqlConnectionString", delegate ()
            {
                string key = string.Format("PlaceholderContextFactory.GetInstanceSqlConnectionString.{0}", code);
                return this.ConnectionStringCache.PerFoundation(key, delegate ()
                {
                    ISettingsResolver settingsResolver = this.IFoundation.Resolve<ISettingsResolver>();

                    string connectionStringKey = string.Format(CommonAssumptions.APP_KEY_SQL_DB_FORMAT, code);
                    string connectionString = settingsResolver.GetSetting(connectionStringKey);

                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new Exception("Connection string not found: " + connectionStringKey);
                    }
                    return connectionString;
                });
            });
        }
    }
}
