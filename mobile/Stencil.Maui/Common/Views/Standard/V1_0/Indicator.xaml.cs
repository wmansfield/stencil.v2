using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class Indicator : ResourceDictionary, IDataViewComponent
    {
        public Indicator()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "indicator";

        private const string TEMPLATE_KEY = "indicator";

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
                IndicatorContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<IndicatorContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new IndicatorContext();
                }
                
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                result.EnsureInteractionsPrepared();

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class IndicatorContext : PreparedBindingContext, IStateResponder
    {
        public IndicatorContext()
            : base(nameof(IndicatorContext))
        {
            this.Visible = true;
            this.MaximumVisible = 6;
            this.IndicatorSize = 14;
        }

        public const string INTERACTION_KEY_SELECTED_POSITION = "position";
        public const string INTERACTION_KEY_VISIBLE = "visible";


        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        private string _color;
        public string Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        private string _selectedColor;
        public string SelectedColor
        {
            get { return _selectedColor; }
            set { SetProperty(ref _selectedColor, value); }
        }

        private bool _visible;
        public bool Visible
        {
            get { return _visible; }
            set { SetProperty(ref _visible, value); }
        }

        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        private int _currentPosition;
        public int CurrentPosition
        {
            get { return _currentPosition; }
            set { SetProperty(ref _currentPosition, value); }
        }

        private int _maximumVisible;
        public int MaximumVisible
        {
            get { return _maximumVisible; }
            set { SetProperty(ref _maximumVisible, value); }
        }

        private int _indicatorSize;
        public int IndicatorSize
        {
            get { return _indicatorSize; }
            set { SetProperty(ref _indicatorSize, value); }
        }

        private int _itemCount;
        public int ItemCount
        {
            get { return _itemCount; }
            set { SetProperty(ref _itemCount, value); }
        }

        protected override void ApplyStateValue(string group, string state_key, string state, string value_key, string value)
        {
            base.ExecuteMethod(nameof(ApplyStateValue), delegate ()
            {
                switch (value_key)
                {
                    case INTERACTION_KEY_VISIBLE:
                        if (!string.IsNullOrEmpty(value))
                        {
                            this.Visible = value.Equals("true", StringComparison.OrdinalIgnoreCase);
                        }
                        break;
                    case INTERACTION_KEY_SELECTED_POSITION:
                        if (int.TryParse(value, out int position))
                        {
                            this.CurrentPosition = position;
                        }
                        break;
                    default:
                        break;
                }
            });
        }
    }
}