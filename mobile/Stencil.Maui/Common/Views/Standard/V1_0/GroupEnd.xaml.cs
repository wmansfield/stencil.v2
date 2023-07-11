using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Stencil.Common.Views;
using Stencil.Maui.Data;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class GroupEnd : ResourceDictionary, IDataViewComponent
    {
        public GroupEnd()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "groupEnd";

        private const string TEMPLATE_KEY = "groupEnd";

        private DataTemplate _dataTemplate;

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
                if (_dataTemplate == null)
                {
                    _dataTemplate = this[TEMPLATE_KEY] as DataTemplate;
                }
                return _dataTemplate;
            });
        }
        public Task<IDataViewItemReference> PrepareBindingContextAsync(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(PrepareBindingContextAsync)}", delegate ()
            {
                GroupEndContext result = null;

                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<GroupEndContext>(configuration_json);
                }

                if(result == null)
                {
                    result = new GroupEndContext();
                }
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;
                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class GroupEndContext : PreparedBindingContext
    {
        public GroupEndContext()
            : base(nameof(GroupEndContext))
        {

        }

        private string _innerColor = AppColors.Transparent;
        public string InnerColor
        {
            get { return _innerColor; }
            set { SetProperty(ref _innerColor, value); }
        }

        private string _outerColor = AppColors.Transparent;
        public string OuterColor
        {
            get { return _outerColor; }
            set { SetProperty(ref _outerColor, value); }
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
    }
}