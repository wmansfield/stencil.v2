using Stencil.Native.Presentation.Menus;
using System;
using System.Collections.Concurrent;

namespace Stencil.Native.Commanding
{
    public class CommandScope : TrackedClass, ICommandScope
    {
        public CommandScope(ICommandProcessor commandProcessor)
            : base(nameof(CommandScope))
        {
            this.CommandProcessor = commandProcessor;
            this.command_data = new ConcurrentDictionary<string, ConcurrentDictionary<string, ICommandField>>(StringComparer.OrdinalIgnoreCase);
        }
        public ICommandProcessor CommandProcessor { get; set; }
        public IMenuEntry TargetMenuEntry { get; set; }


        public ConcurrentDictionary<string, ConcurrentDictionary<string, ICommandField>> command_data { get; }

        public void RegisterCommandField(ICommandField commandField)
        {
            base.ExecuteMethod(nameof(RegisterCommandField), delegate ()
            {
                if(commandField == null || string.IsNullOrWhiteSpace(commandField.FieldName))
                {
                    return; //TODO:COULD:Add Warnings for bad config
                }

                string groupName = commandField.GroupName;
                if(string.IsNullOrWhiteSpace(groupName))
                {
                    groupName = string.Empty;
                }

                ConcurrentDictionary<string, ICommandField> groupFields = null;
                if (!this.command_data.TryGetValue(groupName, out groupFields))
                {
                    this.command_data.TryAdd(groupName, new ConcurrentDictionary<string, ICommandField>());
                    this.command_data.TryGetValue(groupName, out groupFields);// presume it works
                }

                groupFields[commandField.FieldName] = commandField;

            });
        }
    }
}
