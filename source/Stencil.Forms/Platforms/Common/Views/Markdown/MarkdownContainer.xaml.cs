using Newtonsoft.Json;
using Stencil.Common.Markdown;
using Stencil.Forms.Commanding;
using Stencil.Forms.Views.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Forms.Views.Markdown
{
    public partial class MarkdownContainer : ResourceDictionary, IDataViewComponent
    {
        public MarkdownContainer()
        {
            InitializeComponent();
        }


        public const string COMPONENT_NAME = "markdown-container";
        private const string TEMPLATE_KEY = "markdownContainer";

        public bool BindingContextCacheEnabled
        {
            get
            {
                return false;
            }
        }

        public DataTemplate GetDataTemplate()
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(GetDataTemplate)}", delegate ()
            {
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public Task<IDataViewItemReference> PrepareBindingContextAsync(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(PrepareBindingContextAsync)}", delegate ()
            {
                MarkdownContainerContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<MarkdownContainerContext>(configuration_json);
                }
                if (result == null)
                {
                    result = new MarkdownContainerContext();
                }
                result.LinkTappedCommand = new Command<string>(async (destination) => await NativeApplication.CommandProcessor.LinkTapped(destination));
                
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class MarkdownContainerContext : PreparedBindingContext
    {
        public MarkdownContainerContext()
            : base(nameof(MarkdownContainerContext))
        {

        }

        public bool SuppressDivider { get; set; }
        public int FontSize { get; set; }
        public List<MarkdownSection> sections { get; set; }

        [JsonIgnore]
        public ICommand LinkTappedCommand { get; set; }
    }
}