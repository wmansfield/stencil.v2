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

        public const string COMPONENT_NAME = "groupBegin-" + StandardComponentsV1_0.NAME;

        private const string TEMPLATE_KEY = "groupBegin";

        public DataTemplate GetDataTemplate()
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.GetDataTemplate", delegate ()
            {
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public object PrepareData(ICommandScope commandScope, DataTemplateSelector selector, string configuration_json, IDataViewSection[] sections)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.PrepareData", delegate ()
            {
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    return JsonConvert.DeserializeObject<PreparedData>(configuration_json);
                }
                return new PreparedData();
            });
        }
        public class PreparedData : PropertyClass
        {
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
}