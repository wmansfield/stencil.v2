using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

public class ScaffoldingDesignTimeServices : IDesignTimeServices
{
    public ScaffoldingDesignTimeServices()
    {

    }

    protected Dictionary<string, string> _propertyNameCache = new Dictionary<string, string>();
    protected HashSet<string> _properNameTracker = new HashSet<string>();
    protected Dictionary<string, string> _constructorNameCache = new Dictionary<string, string>();
    protected HashSet<string> _constructorTracker = new HashSet<string>();


    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        services.AddHandlebarsScaffolding(options =>
        {
            options.TemplateData = new Dictionary<string, object>()
            {
                {"namespace_context", "Placeholder.Data.Sql"}
            };
        });

        services.AddHandlebarsTransformers(
            entityTypeNameTransformer: this.TransformEntityName,
            constructorTransformer: this.TransformConstructor,
            navPropertyTransformer: this.TransformNavigationProperty);
    }

    protected virtual string TransformEntityName(string entityName)
    {
        _propertyNameCache.Clear();
        _constructorNameCache.Clear();
        _properNameTracker.Clear();
        _constructorTracker.Clear();
        return entityName;
    }

    protected virtual EntityPropertyInfo TransformNavigationProperty(EntityPropertyInfo propertyInfo)
    {
        return this.TransformNaming(_properNameTracker, _propertyNameCache, propertyInfo);
    }
    protected virtual EntityPropertyInfo TransformConstructor(EntityPropertyInfo propertyInfo)
    {
        return this.TransformNaming(_constructorTracker, _constructorNameCache, propertyInfo);
    }
    protected virtual EntityPropertyInfo TransformNaming(HashSet<string> nameTracker, Dictionary<string, string> resolvedCache, EntityPropertyInfo propertyInfo)
    {
        string key = $"{propertyInfo.PropertyType}>{propertyInfo.PropertyName}";
        if (resolvedCache.ContainsKey(key))
        {
            propertyInfo.PropertyName = resolvedCache[key];
        }
        else
        {
            string original = propertyInfo.PropertyName;

            if (propertyInfo.PropertyName.Contains("Navigation"))
            {
                string newName = propertyInfo.PropertyType;
                string[] splits = propertyInfo.PropertyName.Replace("Navigation", "").Split('_');

                if (splits.Length > 2) // convert  entity_id_reasonNavigation -> Entity_Reason
                {
                    char[] suffix = splits[splits.Length - 1].ToArray();
                    suffix[0] = suffix[0].ToString().ToUpper()[0];
                    newName += "_" + new string(suffix);
                }
                if (propertyInfo.PropertyName.Contains("Inverse"))
                {
                    propertyInfo.PropertyName = newName + "_Inbound";
                }
                else
                {
                    propertyInfo.PropertyName = newName;
                }
            }
            else if (propertyInfo.PropertyName.Contains("_"))
            {
                string[] splits = propertyInfo.PropertyName.Split('_');

                propertyInfo.PropertyName = string.Concat(splits.Select(x => x.Substring(0, 1).ToUpperInvariant() + x.Substring(1)));
                if (propertyInfo.PropertyName.Contains("Inverse"))
                {
                    propertyInfo.PropertyName += "_Inbound";
                }
            }
            else if (propertyInfo.PropertyName[0].ToString() == propertyInfo.PropertyName[0].ToString().ToLower())
            {
                string proposedName = propertyInfo.PropertyName.Substring(0, 1).ToUpperInvariant() + propertyInfo.PropertyName.Substring(1);
                propertyInfo.PropertyName = proposedName;
            }

            resolvedCache[key] = propertyInfo.PropertyName;
        }

        return propertyInfo;
    }

}
