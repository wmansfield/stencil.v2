using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class Spacer : ResourceDictionary, IDataViewComponent
    {
        public Spacer()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "spacer";

        private const string TEMPLATE_KEY = "spacer";

        public bool BindingContextCacheEnabled
        {
            get
            {
                return true;
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
                SpacerContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<SpacerContext>(configuration_json);
                }

                if(result == null)
                {
                    result = new SpacerContext();
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class SpacerContext : PreparedBindingContext
    {
        public SpacerContext()
            : base(nameof(SpacerContext))
        {

        }

        public int Height { get; set; }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }
    }
}