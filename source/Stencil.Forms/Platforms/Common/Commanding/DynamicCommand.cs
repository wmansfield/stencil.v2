using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Forms.Screens;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Stencil.Forms.Commanding
{
    public abstract class DynamicCommand<TAPI> : TrackedClass<TAPI>
        where TAPI : StencilAPI
    {
        public DynamicCommand(TAPI api, string trackPrefix)
            : base(api, trackPrefix)
        {

        }

        public virtual string ExtractValue(ICommandScope scope, string group, string name)
        {
            return base.ExecuteFunction(nameof(ExtractValue), delegate ()
            {
                ICommandField commandField = this.ExtractCommandField(scope, group, name);
                if (commandField != null)
                {
                    return commandField.GetFieldValue();
                }

                return null;
            });
        }
        public virtual Task<string> ValidateUserInputValuesAsync(ICommandScope scope, string group, params string[] fieldNames)
        {
            return base.ExecuteFunctionAsync(nameof(ValidateUserInputValuesAsync), async delegate ()
            {
                ConcurrentDictionary<string, ICommandField> values = this.ExtractCommandGroup(scope, group);
                if (values != null)
                {
                    foreach (string field in fieldNames)
                    {
                        if(values.TryGetValue(field, out ICommandField commandField))
                        {
                            return await commandField.ValidateUserInputAsync();
                        }
                        else
                        {
                            return "No value provided.";//TODO:MUST:LOCALIZE
                        }
                    }
                }

                return null;
            });
        }
        public virtual ICommandField ExtractCommandField(ICommandScope scope, string group, string name)
        {
            return base.ExecuteFunction(nameof(ExtractCommandField), delegate ()
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return null;
                }
                ConcurrentDictionary<string, ICommandField> values = this.ExtractCommandGroup(scope, group);
                if (values != null)
                {
                    if (values.TryGetValue(name, out ICommandField found))
                    {
                        return found;
                    }
                }

                return null;
            });
        }
        public virtual ConcurrentDictionary<string, ICommandField> ExtractCommandGroup(ICommandScope scope, string group)
        {
            return base.ExecuteFunction(nameof(ExtractCommandGroup), delegate ()
            {
                if (scope != null && group != null)
                {
                    if (scope.command_data.TryGetValue(group, out ConcurrentDictionary<string, ICommandField> values))
                    {
                        return values;
                    }
                }

                return null;
            });
        }

        protected virtual NavigationData ParseNavigationData(object commandParameter)
        {
            return base.ExecuteFunction(nameof(ParseNavigationData), delegate ()
            {
                if (commandParameter != null)
                {
                    NavigationData navigationData = commandParameter as NavigationData;
                    if (navigationData != null)
                    {
                        return navigationData;
                    }

                    string stringContents = commandParameter.ToString();
                    if (stringContents.StartsWith("{"))
                    {
                        return JsonConvert.DeserializeObject<NavigationData>(stringContents);
                    }
                    else
                    {
                        return new NavigationData()
                        {
                            screen_name = stringContents
                        };
                    }
                }
                return null;
            });
        }
    }
}
