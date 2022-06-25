using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using Stencil.Forms.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class GroupBegin : ResourceDictionary, IDataViewComponent
    {
        public GroupBegin()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "groupBegin";

        private const string TEMPLATE_KEY = "groupBegin";

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
                if(_dataTemplate == null)
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
                GroupBeginContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<GroupBeginContext>(configuration_json);
                }

                if(result == null)
                {
                    result = new GroupBeginContext();
                }

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }
    }

    public class GroupBeginContext : PreparedBindingContext
    {
        public GroupBeginContext()
            : base(nameof(GroupBeginContext))
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
    }
}