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

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class TripleStackView : ResourceDictionary, IDataViewComponent
    {
        public TripleStackView()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "tripleStackView";

        private const string TEMPLATE_KEY = "tripleStackView";

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
                TripleStackViewContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<TripleStackViewContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new TripleStackViewContext();
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

    public class TripleStackViewContext : PreparedBindingContext
    {
        public TripleStackViewContext()
            : base(nameof(TripleStackViewContext))
        {
            this.HeightRequest = -1;
            this.Column1Config = new ColumnConfig()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill
            };
            this.Column2Config = new ColumnConfig()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill
            };
            this.Column3Config = new ColumnConfig()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill
            };
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