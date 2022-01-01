using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Platform;
using Stencil.Native.Resourcing;
using System;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class PrimaryButton : ResourceDictionary, IDataViewComponent
    {
        public PrimaryButton()
        {
            InitializeComponent();
        }



        public const string COMPONENT_NAME = "primaryButton";

        private const string TEMPLATE_KEY = "primaryButton";

        public bool PreparedDataCacheDisabled
        {
            get
            {
                return false;
            }
        }

        public DataTemplate GetDataTemplate()
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.GetDataTemplate", delegate ()
            {
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public object PrepareData(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
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
                View view = (sender as View);
                IDataViewItem dataViewItem = view.BindingContext as IDataViewItem;
                if (dataViewItem != null)
                {
                    DependencyService.Get<IKeyboardManager>()?.TryHideKeyboard();

                    PreparedData preparedData = dataViewItem.PreparedData as PreparedData;
                    try
                    {
                        preparedData.UIButtonBackgroundColor = AppColors.Primary400;
                        if (!string.IsNullOrWhiteSpace(preparedData?.CommandName))
                        {
                            if (preparedData?.CommandScope?.CommandProcessor != null)
                            {
                                await preparedData.CommandScope.CommandProcessor.ExecuteCommandAsync(preparedData.CommandScope, preparedData.CommandName, preparedData.CommandParameter);
                            }
                        }
                    }
                    finally
                    {
                        preparedData.UIButtonBackgroundColor = AppColors.Primary900;
                    }
                    
                }
            });
            
        }
        public class PreparedData : PropertyClass
        {
            public string Text { get; set; }
            public string CommandName { get; set; }
            public string CommandParameter { get; set; }

            private string _uiButtonBackgroundColor = AppColors.Primary900;
            public string UIButtonBackgroundColor
            {
                get { return _uiButtonBackgroundColor; }
                set { SetProperty(ref _uiButtonBackgroundColor, value); }
            }

            private string _backgroundColor;
            public string BackgroundColor
            {
                get { return _backgroundColor; }
                set { SetProperty(ref _backgroundColor, value); }
            }

            private Thickness _padding = new Thickness(20, 0);
            public Thickness Padding
            {
                get { return _padding; }
                set { SetProperty(ref _padding, value); }
            }
            

            [JsonIgnore]
            public ICommandScope CommandScope { get; set; }
        }

        
    }
}