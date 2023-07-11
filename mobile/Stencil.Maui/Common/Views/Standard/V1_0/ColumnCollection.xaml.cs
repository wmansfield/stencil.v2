using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Maui.Commanding;
using Stencil.Maui.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using System;
using static System.Collections.Specialized.BitVector32;
using Stencil.Common.Views;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class ColumnCollection : ResourceDictionary, IDataViewComponent
    {
        public ColumnCollection()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "columnCollection";

        private const string TEMPLATE_KEY = "columnCollection";

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
                ColumnCollectionContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<ColumnCollectionContext>(configuration_json);
                }
                if (result == null)
                {
                    result = new ColumnCollectionContext();
                }

                if(result.HeightRequest <= 0)
                {
                    result.HeightRequest = 120;
                }
                if (result.ContentWidth <= 0)
                {
                    result.ContentWidth = 120;
                }
                if (result.ContentHeight <= 0)
                {
                    result.ContentHeight = result.HeightRequest - 4; // don't set exact, pixel math
                }
                

                List<INestedDataViewModel> cells = new List<INestedDataViewModel>();

                if (dataViewItem?.Sections != null)
                {
                    foreach (IDataViewSection section in dataViewItem.Sections)
                    {
                        if (section.ViewItems?.Length > 0)
                        {
                            StandardNestedDataViewModel childViewModel = new StandardNestedDataViewModel(commandScope.CommandProcessor, selector)
                            {
                                MainItemsUnFiltered = new ObservableCollection<IDataViewItem>(section.ViewItems),
                                MainItemsFiltered = new ObservableCollection<object>(section.ViewItems.Select(x => x.PreparedContext))
                            };

                            // extract filters or augmentations
                            await childViewModel.ExtractAndPrepareExtensionsAsync();

                            // apply page visuals
                            if (section.VisualConfig != null)
                            {
                                childViewModel.BackgroundImage = section.VisualConfig.BackgroundImage;

                                if (!string.IsNullOrWhiteSpace(section.VisualConfig.BackgroundColor))
                                {
                                    childViewModel.BackgroundColor = Color.FromArgb(section.VisualConfig.BackgroundColor);
                                }
                                if (section.VisualConfig.BackgroundBrush != null)
                                {
                                    childViewModel.BackgroundBrush = section.VisualConfig.BackgroundBrush.ToBrush();
                                }
                                childViewModel.Padding = section.VisualConfig.Padding.ToThickness();
                            }

                            await childViewModel.Initialize();

                            cells.Add(childViewModel);
                        }
                    }
                }

                result.Cells = cells.ToArray();
                
                result.DataTemplateSelector = selector;

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return result;
            });
        }


    }


    public class ColumnCollectionContext : PreparedBindingContext
    {
        public ColumnCollectionContext()
            : base(nameof(ColumnCollectionContext))
        {
            this.HeightRequest = 300;
            this.VerticalOptions = LayoutOptions.Fill;
        }


        public string BackgroundColor { get; set; }

        public double HeightRequest { get; set; }
        public LayoutOptions VerticalOptions { get; set; }
        

        private double _contentWidth;
        public double ContentWidth
        {
            get { return _contentWidth; }
            set { SetProperty(ref _contentWidth, value); }
        }

        private double _contentHeight;
        public double ContentHeight
        {
            get { return _contentHeight; }
            set { SetProperty(ref _contentHeight, value); }
        }

        private ThicknessInfo _margin;
        public ThicknessInfo Margin
        {
            get { return _margin; }
            set
            {
                if (SetProperty(ref _margin, value))
                {
                    this.UIMargin = value.ToThickness();
                }
            }
        }
        private Thickness _uiMargin = new Thickness();
        public Thickness UIMargin
        {
            get { return _uiMargin; }
            protected set { SetProperty(ref _uiMargin, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public DataTemplateSelector DataTemplateSelector { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public INestedDataViewModel[] Cells { get; set; }

    }

}