using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

public class ScaffoldingDesignTimeServices : IDesignTimeServices
{
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
            entityTypeNameTransformer: this.transformEntityName,
            constructorTransformer: this.transformConstructor,
            navPropertyTransformer: this.transformNavigationProperty);
    }

    private string transformEntityName(string entityName)
    {
        _propertyNameCache.Clear();
        _constructorNameCache.Clear();
        return entityName;
    }

    private HashSet<string> _propertyNameCache = new HashSet<string>();
    private HashSet<string> _constructorNameCache = new HashSet<string>();

    private EntityPropertyInfo transformNavigationProperty(EntityPropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyName.Contains("Navigation"))
        {
            propertyInfo.PropertyName = this.GetIncrementalPropertyName(propertyInfo.PropertyType);
        }
        if (propertyInfo.PropertyName.Contains("_"))
        {
            string[] splits = propertyInfo.PropertyName.Split('_');
            propertyInfo.PropertyName = string.Concat(splits.Select(x => x.Substring(0, 1).ToUpperInvariant() + x.Substring(1)));
        }

        if (propertyInfo.PropertyName[0].ToString() == propertyInfo.PropertyName[0].ToString().ToLower())
        {
            string proposedName = propertyInfo.PropertyName.Substring(0, 1).ToUpperInvariant() + propertyInfo.PropertyName.Substring(1);
            propertyInfo.PropertyName = this.GetIncrementalPropertyName(proposedName);
        }

        return propertyInfo;
    }
    private string GetIncrementalPropertyName(string name)
    {
        string newName = null;
        bool added = false;
        int counter = 0;
        do
        {
            newName = name;
            if (counter > 0)
            {
                newName += counter.ToString();
            }
            added = _propertyNameCache.Add(newName);
            counter++;
        }
        while (!added);
        return newName;
    }

    private EntityPropertyInfo transformConstructor(EntityPropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyName.Contains("Navigation"))
        {
            string newName = null;
            bool added = false;
            int counter = 0;
            do
            {
                newName = propertyInfo.PropertyType;
                if (counter > 0)
                {
                    newName += counter.ToString();
                }
                added = _constructorNameCache.Add(newName);
                counter++;
            }
            while (!added);
            propertyInfo.PropertyName = newName;
        }
        return propertyInfo;
    }
}
