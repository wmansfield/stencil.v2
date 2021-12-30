using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Native.Views.Markdown
{
    public partial class MarkdownContainer : ResourceDictionary, IDataViewComponent
    {
        public MarkdownContainer()
        {
            InitializeComponent();
        }


        public const string COMPONENT_NAME = "markdown-container";
        private const string TEMPLATE_KEY = "markdownContainer";

        public DataTemplate GetDataTemplate()
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.GetDataTemplate", delegate ()
            {
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public object PrepareData(ICommandScope commandScope, DataTemplateSelector selector, string configuration_json, IDataViewSection[] sections)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.PrepareData", delegate ()
            {
                PreparedData result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<PreparedData>(configuration_json);
                }
                if (result == null)
                {
                    result = new PreparedData();
                }
                result.LinkTappedCommand = new Command<string>(async (destination) => await NativeApplication.CommandProcessor.LinkTapped(destination));
                result.CommandScope = commandScope;
                return result;
            });
        }

        public class PreparedData
        {
            public bool SuppressDivider { get; set; }
            public int FontSize { get; set; }
            public List<MarkdownSection> sections { get; set; }

            [JsonIgnore]
            public ICommand LinkTappedCommand { get; set; }

            [JsonIgnore]
            public ICommandScope CommandScope { get; set; }
        }

    }
}