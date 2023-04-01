using Stencil.Maui.Commanding;
using Stencil.Maui.Commanding.Commands;
using Stencil.Maui.Views;

namespace Starter.App.Commands
{
    public class StarterCommandProcessor : StarterTrackedClass, ICommandProcessor
    {
        public StarterCommandProcessor()
            : base(nameof(StarterCommandProcessor))
        {
            _appCommands = new Dictionary<string, Func<IAppCommand>>(StringComparer.OrdinalIgnoreCase)
            {
                { WellKnownCommands.CHAINED_COMMAND, new Func<IAppCommand>(() => new ChainedCommand()) },
                { WellKnownCommands.APP_NAVIGATE_PUSH, new Func<IAppCommand>(() => new NavigatePushCommand()) },
                { WellKnownCommands.APP_NAVIGATE_POP, new Func<IAppCommand>(() => new NavigatePopCommand()) },
                { WellKnownCommands.APP_NAVIGATE_ROOT, new Func<IAppCommand>(() => new NavigateRootCommand()) },
                { WellKnownCommands.COPY_CLIPBOARD, new Func<IAppCommand>(() => new CopyToClipboardCommand()) },
                { WellKnownCommands.STATE_CHANGE, new Func<IAppCommand>(() => new ChangeStateCommand()) },

            };

            _dataCommands = new Dictionary<string, Func<IDataCommand>>(StringComparer.OrdinalIgnoreCase)
            {
                //...
            };
        }

        private Dictionary<string, Func<IAppCommand>> _appCommands;
        private Dictionary<string, Func<IDataCommand>> _dataCommands;

        public bool IsDataCommand(string commandName)
        {
            return base.ExecuteFunction(nameof(IsDataCommand), delegate ()
            {
                return _dataCommands.ContainsKey(commandName);

            });
        }
        public bool IsAppCommand(string commandName)
        {
            return base.ExecuteFunction(nameof(IsAppCommand), delegate ()
            {
                return _appCommands.ContainsKey(commandName);
            });
        }

        public Task<bool> ExecuteCommandAsync(ICommandScope commandScope, string commandName, object commandParameter, IDataViewModel dataViewModel, CancellationToken token = default)
        {
            return base.ExecuteFunctionAsync(nameof(ExecuteCommandAsync), async delegate ()
            {
                if (string.IsNullOrWhiteSpace(commandName))
                {
                    return false;
                }

                IAppCommand command = null;
                if (_appCommands.TryGetValue(commandName, out Func<IAppCommand> commandFactory))
                {
                    command = commandFactory();
                }
                if (command != null)
                {
                    string message = await command.CanExecuteAsync(commandScope, dataViewModel);
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        if (command.AlertErrors == true || commandScope?.AlertErrors == true)
                        {
                            await this.API.Alerts.AlertAsync(message);
                        }
                        this.API.Logger.LogDebug(message);
                        return false;
                    }
                    else
                    {
                        return await command.ExecuteAsync(commandScope, commandParameter, dataViewModel, token);
                    }
                }
                else
                {
                    this.LogTrace($"The command `{commandName}` was requested but was not found. Check Command Factory.");
                }

                return false;
            });
        }

        public Task<object> ExecuteDataCommandAsync(ICommandScope commandScope, string commandName, object commandParameter, IDataViewModel dataViewModel, CancellationToken token = default)
        {
            return base.ExecuteFunctionAsync<object>(nameof(ExecuteDataCommandAsync), async delegate ()
            {
                if (string.IsNullOrWhiteSpace(commandName))
                {
                    return false;
                }

                IDataCommand command = null;
                if (_dataCommands.TryGetValue(commandName, out Func<IDataCommand> commandFactory))
                {
                    command = commandFactory();
                }
                if (command != null)
                {
                    string message = await command.CanExecuteAsync(commandScope, dataViewModel);
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        if (command.AlertErrors == true || commandScope?.AlertErrors == true)
                        {
                            await this.API.Alerts.AlertAsync(message);
                        }
                        this.API.Logger.LogDebug(message);
                        return null;
                    }
                    else
                    {
                        object result = await command.ExecuteAsync(commandScope, commandParameter, dataViewModel, token);
                        return result;
                    }
                }
                else
                {
                    this.LogTrace($"The command `{commandName}` was requested but was not found. Check Command Factory.");
                }
                return null;
            });
        }

        public Task LinkTapped(string destination)
        {
            return base.ExecuteMethodAsync(nameof(LinkTapped), async delegate ()
            {
                if (destination.StartsWith("http"))
                {
                    await Launcher.OpenAsync(destination);
                }
            });
        }

    }
}
