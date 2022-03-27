﻿using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Resourcing;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class H1 : ResourceDictionary, IDataViewComponent
    {
        public H1()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "h1";
        public const string INTERACTION_KEY_TEXT = "text";
        public const string INTERACTION_KEY_VISIBLE = "visible";

        private const string TEMPLATE_KEY = "h1";

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
                H1Context result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<H1Context>(configuration_json);
                }

                if(result == null)
                {
                    result = new H1Context();
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.PrepareInteractions();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class H1Context : PreparedBindingContext, IStateResponder
    {
        public H1Context()
            : base(nameof(H1Context))
        {

        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _textColor;
        public string TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private Thickness _padding;
        public Thickness Padding
        {
            get { return _padding; }
            set { SetProperty(ref _padding, value); }
        }

        protected override void ApplyStateValue(string group, string state_key, string state, string value_key, string value)
        {
            base.ExecuteMethod(nameof(ApplyStateValue), delegate ()
            {
                switch (value_key)
                {
                    case H1.INTERACTION_KEY_TEXT:
                    default:
                        this.Text = value;
                        break;
                }
            });
        }
    }
}