using Newtonsoft.Json;
using Stencil.Native.Commanding;
using System;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class Button : ResourceDictionary, IDataViewComponent
    {
        public Button()
        {
            InitializeComponent();
        }



        public const string COMPONENT_NAME = "button-" + StandardComponentsV1_0.NAME;

        private const string TEMPLATE_KEY = "button";

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
                if(result == null)
                {
                    result = new PreparedData();
                }
                result.CommandScope = commandScope;
                return result;
            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync($"{COMPONENT_NAME}.Button_Clicked", async delegate ()
            {
                IDataViewItem dataViewItem = (sender as Xamarin.Forms.Button).BindingContext as IDataViewItem;
                if (dataViewItem != null)
                {
                    PreparedData preparedData = dataViewItem.PreparedData as PreparedData;
                    if (!string.IsNullOrWhiteSpace(preparedData?.CommandName))
                    {
                        if(preparedData?.CommandScope?.CommandProcessor != null)
                        {
                            await preparedData.CommandScope.CommandProcessor.ExecuteCommandAsync(preparedData.CommandScope, preparedData.CommandName, preparedData.CommandParameter);
                        }
                    }
                }
            });
            
        }
        public class PreparedData
        {
            public string Text { get; set; }
            public string CommandName { get; set; }
            public string CommandParameter { get; set; }

            [JsonIgnore]
            public ICommandScope CommandScope { get; set; }
        }

        
    }
}