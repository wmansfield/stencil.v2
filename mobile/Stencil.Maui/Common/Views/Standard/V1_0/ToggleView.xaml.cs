using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Data;
using Stencil.Maui.Resourcing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class ToggleView : ResourceDictionary, IDataViewComponent
    {
        public ToggleView()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "toggleView";

        private const string TEMPLATE_KEY = "toggleView";

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
            return CoreUtility.ExecuteFunctionAsync<IDataViewItemReference>($"{COMPONENT_NAME}.{nameof(PrepareBindingContextAsync)}", async delegate ()
            {
                ToggleViewContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<ToggleViewContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new ToggleViewContext();
                }

                if(result.HeightRequest == 0)
                {
                    result.HeightRequest = -1;
                }
                if(result.Content1Config == null)
                {
                    result.Content1Config = new ToggleConfig()
                    {
                        Visible = true
                    };
                }
                if (result.Content2Config == null)
                {
                    result.Content2Config = new ToggleConfig()
                    {
                        Visible = true
                    };
                }
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.EnsureInteractionsPrepared();

                if (dataViewItem?.EncapsulatedItems != null)
                {
                    IResolvableTemplateSelector resolvableSelector = selector as IResolvableTemplateSelector;
                    //TODO:MUST: Force IResolvableTemplateSelector, null is no longer allowed

                    for (int i = 0; i < dataViewItem.EncapsulatedItems.Length; i++)
                    {
                        IDataViewItem nestedViewItem = dataViewItem.EncapsulatedItems[i];

                        IDataViewComponent dataViewComponent = resolvableSelector.ResolveTemplateAndPrepareData(nestedViewItem);

                        await dataViewModel.ExtractAndPrepareExtensionsAsync(nestedViewItem);

                        if (i == 0)
                        {
                            View view = dataViewComponent.GetDataTemplate().CreateContent() as View;
                            view.BindingContext = nestedViewItem.PreparedContext;
                            result.FirstContent = view;
                        }
                        else if(i == 1)
                        {
                            View view = dataViewComponent.GetDataTemplate().CreateContent() as View;
                            view.BindingContext = nestedViewItem.PreparedContext;
                            result.SecondContent = view;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
               
                result.DataTemplateSelector = selector;

                return result;
            });
        }
        
    }

    public class ToggleViewContext : PreparedBindingContext, IStateResponder
    {
        public ToggleViewContext()
            : base(nameof(ToggleViewContext))
        {
        }

        public const string INTERACTION_KEY_VISIBLE_CONTENT = "visible_content";

        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }
        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private int _heightRequest;
        public int HeightRequest
        {
            get { return _heightRequest; }
            set { SetProperty(ref _heightRequest, value); }
        }

        private ToggleConfig _content1Config;
        public ToggleConfig Content1Config
        {
            get { return _content1Config; }
            set { SetProperty(ref _content1Config, value); }
        }

        private ToggleConfig _content2Config;
        public ToggleConfig Content2Config
        {
            get { return _content2Config; }
            set { SetProperty(ref _content2Config, value); }
        }

        [JsonIgnore]
        public object FirstContent { get; set; }

        [JsonIgnore]
        public object SecondContent { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public DataTemplateSelector DataTemplateSelector { get; set; }

        public string CommandName { get; set; }
        public string CommandParameter { get; set; }


        protected override void ApplyStateValue(string group, string state_key, string state, string value_key, string value)
        {
            base.ExecuteMethod(nameof(ApplyStateValue), delegate ()
            {
                switch (value_key)
                {
                    case INTERACTION_KEY_VISIBLE_CONTENT:
                        if (!string.IsNullOrEmpty(value))
                        {
                            if(value == "1")
                            {
                                this.Content1Config.Visible = true;
                                this.Content2Config.Visible = false;
                            }
                            else if (value == "2")
                            {
                                this.Content1Config.Visible = false;
                                this.Content2Config.Visible = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            });
        }
    }
}