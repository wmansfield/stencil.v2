using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Stencil.Native.Commanding
{
    public abstract class BaseAppCommand<TAPI> : TrackedClass<TAPI>, IAppCommand
        where TAPI : StencilAPI
    {
        public BaseAppCommand(TAPI api, string trackPrefix)
            : base(api, trackPrefix)
        {

        }

        public abstract Task<string> CanExecuteAsync(ICommandScope commandScope);

        public abstract Task<bool> ExecuteAsync(ICommandScope commandScope, object commandParameter);

        public string ExtractValue(ICommandScope scope, string group, string name)
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
        public Task<string> ValidateUserInputValuesAsync(ICommandScope scope, string group, params string[] fieldNames)
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
        public ICommandField ExtractCommandField(ICommandScope scope, string group, string name)
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
        public ConcurrentDictionary<string, ICommandField> ExtractCommandGroup(ICommandScope scope, string group)
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
    }
}
