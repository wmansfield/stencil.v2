using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Data;
using Stencil.Forms.Resourcing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class TriColumnView : ResourceDictionary, IDataViewComponent
    {
        public TriColumnView()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "triColumnView";

        private const string TEMPLATE_KEY = "triColumnView";

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
                TriColumnViewContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<TriColumnViewContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new TriColumnViewContext();
                }
                
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                if (dataViewItem?.EncapsulatedItems != null)
                {
                    IResolvableTemplateSelector resolvableSelector = selector as IResolvableTemplateSelector;
                    //TODO:MUST: Force IResolvableTemplateSelector, null is no longer allowed

                    for (int i = 0; i < dataViewItem.EncapsulatedItems.Length; i++)
                    {
                        IDataViewItem nestedViewItem = dataViewItem.EncapsulatedItems[i];

                        IDataViewComponent dataViewComponent = await resolvableSelector.ResolveTemplateAndPrepareDataAsync(nestedViewItem);

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
                        else if (i == 2)
                        {
                            View view = dataViewComponent.GetDataTemplate().CreateContent() as View;
                            view.BindingContext = nestedViewItem.PreparedContext;
                            result.ThirdContent = view;
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

    public class TriColumnViewContext : PreparedBindingContext
    {
        public TriColumnViewContext()
            : base(nameof(TriColumnViewContext))
        {
            this.HeightRequest = -1;
            this.Column1Config = new ColumnConfig();
            this.Column2Config = new ColumnConfig();
            this.Column3Config = new ColumnConfig();
        }

        private ColumnConfig _column1Config;
        public ColumnConfig Column1Config
        {
            get { return _column1Config; }
            set { SetProperty(ref _column1Config, value); }
        }

        private ColumnConfig _column2Config;
        public ColumnConfig Column2Config
        {
            get { return _column2Config; }
            set { SetProperty(ref _column2Config, value); }
        }

        private ColumnConfig _column3Config;
        public ColumnConfig Column3Config
        {
            get { return _column3Config; }
            set { SetProperty(ref _column3Config, value); }
        }

        private int _heightRequest;
        public int HeightRequest
        {
            get { return _heightRequest; }
            set { SetProperty(ref _heightRequest, value); }
        }


        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        [JsonIgnore]
        public object FirstContent { get; set; }

        [JsonIgnore]
        public object SecondContent { get; set; }
        
        [JsonIgnore]
        public object ThirdContent { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public DataTemplateSelector DataTemplateSelector { get; set; }

        public string CommandName { get; set; }
        public string CommandParameter { get; set; }

    }
}