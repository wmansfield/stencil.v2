using Newtonsoft.Json;
using Stencil.Maui;
using Stencil.Maui.Commanding;
using Stencil.Maui.Views;
using Stencil.Maui.Views.Standard;

namespace Starter.App.Views.V1
{
    public partial class SampleView : ResourceDictionary, IDataViewComponent
    {
        public SampleView()
        {
            this.InitializeComponent();
        }


        public const string COMPONENT_NAME = "sampleView";

        private const string TEMPLATE_KEY = "sampleView";

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
                SampleViewContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<SampleViewContext>(configuration_json);
                }
                if (result == null)
                {
                    result = new SampleViewContext();
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.PrepareInteractions();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        private async void Tapped(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync(nameof(Tapped), async delegate ()
            {
                View view = (sender as View);
                SampleViewContext context = view?.BindingContext as SampleViewContext;
                if (context != null)
                {
                    if (context.CommandScope?.CommandProcessor != null)
                    {
                        await context.CommandScope.CommandProcessor.ExecuteCommandAsync(context.CommandScope, context.CommandName, context.CommandParameter, context?.DataViewItem?.DataViewModel);
                    }
                }
            });
        }
    }

    public class SampleViewContext : PreparedBindingContext, IStateResponder
    {
        public SampleViewContext()
            : base("SampleViewContext")
        {

        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private Thickness _margin = new Thickness();
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        private string _textColor;
        public string TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

        public string CommandName { get; set; }
        public string CommandParameter { get; set; }

    }
}