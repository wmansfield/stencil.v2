using Newtonsoft.Json;
using Stencil.Native.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class GroupBegin : ResourceDictionary, IDataViewComponent
    {
        public GroupBegin()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "groupBegin";

        private const string TEMPLATE_KEY = "groupBegin";

        public bool BindingContextCacheEnabled
        {
            get
            {
                return false;
            }
        }

        public DataTemplate GetDataTemplate()
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(GetDataTemplate)}", delegate ()
            {
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public IDataViewItemReference PrepareBindingContext(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(PrepareBindingContext)}", delegate ()
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

                return result;
            });
        }
    }

    public class GroupBeginContext : PreparedBingingContext
    {
        public GroupBeginContext()
            : base(nameof(GroupBeginContext))
        {

        }

        private string _innerColor;
        public string InnerColor
        {
            get { return _innerColor; }
            set { SetProperty(ref _innerColor, value); }
        }

        private string _outerColor;
        public string OuterColor
        {
            get { return _outerColor; }
            set { SetProperty(ref _outerColor, value); }
        }
    }
}