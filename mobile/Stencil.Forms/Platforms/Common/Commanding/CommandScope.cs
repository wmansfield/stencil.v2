using Stencil.Forms.Presentation.Menus;
using System;
using System.Collections.Concurrent;

namespace Stencil.Forms.Commanding
{
    public class CommandScope : TrackedClass, ICommandScope
    {
        public CommandScope(ICommandProcessor commandProcessor)
            : base(nameof(CommandScope))
        {
            this.CommandProcessor = commandProcessor;
            this.ExchangeData = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.CommandData = new ConcurrentDictionary<string, ConcurrentDictionary<string, ICommandField>>(StringComparer.OrdinalIgnoreCase);
        }
        public ICommandProcessor CommandProcessor { get; set; }
        public IMenuEntry TargetMenuEntry { get; set; }

        public bool AlertErrors { get; set; }

        public ConcurrentDictionary<string, ConcurrentDictionary<string, ICommandField>> CommandData { get; }

        public ConcurrentDictionary<string, object> ExchangeData { get; }


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
                if (!this.CommandData.TryGetValue(groupName, out groupFields))
                {
                    this.CommandData.TryAdd(groupName, new ConcurrentDictionary<string, ICommandField>());
                    this.CommandData.TryGetValue(groupName, out groupFields);// presume it works
                }

                groupFields[commandField.FieldName] = commandField;

            });
        }
    }
}
