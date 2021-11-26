using Newtonsoft.Json;
using Stencil.Native.Commanding;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class SimpleEntry : ResourceDictionary, IDataViewComponent
    {
        public SimpleEntry()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "simpleEntry-" + StandardComponentsV1_0.NAME;

        private const string TEMPLATE_KEY = "simpleEntry";

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
                PreparedData result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<PreparedData>(configuration_json);
                }
                if (result == null)
                {
                    result = new PreparedData();
                }
                result.CommandScope = commandScope;

                commandScope.RegisterCommandField(result);

                return result;
            });
        }

        public class PreparedData : TrackedClass, ICommandField
        {
            public PreparedData()
                : base($"{nameof(SimpleEntry)}.{nameof(PreparedData)}")
            {

            }

            private string _input;
            public string Input
            {
                get { return _input; }
                set { SetProperty(ref _input, value); }
            }

            public string Inputs { get; set; }
            public string Placeholder { get; set; }
            public bool IsPassword { get; set; }
            public bool IsRequired { get; set; }


            public string GroupName { get; set; }
            public string FieldName { get; set; }

            public string GetFieldValue()
            {
                return this.Input;
            }
            public void SetFieldValue(string value)
            {
                this.Input = value;
            }
            public Task<string> ValidateUserInputAsync()
            {
                return base.ExecuteFunction(nameof(ValidateUserInputAsync), delegate ()
                {
                    if(this.IsRequired)
                    {
                        string value = this.GetFieldValue();
                        if(string.IsNullOrWhiteSpace(value))
                        {
                            return Task.FromResult("Value required");//TODO:MUST: Localize
                        }
                    }
                    return Task.FromResult((string)null);
                });
            }

            [JsonIgnore]
            public ICommandScope CommandScope { get; set; }
        }
    }
}
