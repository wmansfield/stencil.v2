using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using db = Placeholder.Data.Sql.Models;

namespace Placeholder.Data.Sql
{
    public static class DatabaseExtensions
    {
        
        public static void InvalidateSync(this db.Account model, string agent, string reason)
        {
            if (model != null)
            {
                model.sync_attempt_utc = null;
                model.sync_success_utc = null;
                model.sync_hydrate_utc = null;
                model.sync_log = reason;
                model.sync_invalid_utc = DateTime.UtcNow;
                model.sync_agent = agent;
            }
        }
        
        public static void InvalidateSync(this db.Shop model, string agent, string reason)
        {
            if (model != null)
            {
                model.sync_attempt_utc = null;
                model.sync_success_utc = null;
                model.sync_hydrate_utc = null;
                model.sync_log = reason;
                model.sync_invalid_utc = DateTime.UtcNow;
                model.sync_agent = agent;
            }
        }
        
        public static void InvalidateSync(this db.ShopIsolated model, string agent, string reason)
        {
            if (model != null)
            {
                model.sync_attempt_utc = null;
                model.sync_success_utc = null;
                model.sync_hydrate_utc = null;
                model.sync_log = reason;
                model.sync_invalid_utc = DateTime.UtcNow;
                model.sync_agent = agent;
            }
        }
        
        public static void InvalidateSync(this db.ShopAccount model, string agent, string reason)
        {
            if (model != null)
            {
                model.sync_attempt_utc = null;
                model.sync_success_utc = null;
                model.sync_hydrate_utc = null;
                model.sync_log = reason;
                model.sync_invalid_utc = DateTime.UtcNow;
                model.sync_agent = agent;
            }
        }
        
        public static void InvalidateSync(this db.ShopSetting model, string agent, string reason)
        {
            if (model != null)
            {
                model.sync_attempt_utc = null;
                model.sync_success_utc = null;
                model.sync_hydrate_utc = null;
                model.sync_log = reason;
                model.sync_invalid_utc = DateTime.UtcNow;
                model.sync_agent = agent;
            }
        }
        
        public static void InvalidateSync(this db.Company model, string agent, string reason)
        {
            if (model != null)
            {
                model.sync_attempt_utc = null;
                model.sync_success_utc = null;
                model.sync_hydrate_utc = null;
                model.sync_log = reason;
                model.sync_invalid_utc = DateTime.UtcNow;
                model.sync_agent = agent;
            }
        }
        
    }
}
