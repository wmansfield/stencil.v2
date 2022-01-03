﻿using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class HeaderBackBar : ResourceDictionary, IDataViewComponent
    {
        public HeaderBackBar()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "headerBackBar";

        private const string TEMPLATE_KEY = "headerBackBar";

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
                if (string.IsNullOrWhiteSpace(result.CommandName))
                {
                    result.CommandName = "app.navigate.pop";//TODO:MUST:Magic String
                    result.CommandParameter = null;
                }
                if(string.IsNullOrWhiteSpace(result.TextColor))
                {
                    result.TextColor = AppColors.TextOverPrimary;
                }
                if (string.IsNullOrWhiteSpace(result.BackgroundColor))
                {
                    result.BackgroundColor = AppColors.Primary900;
                }
                if (string.IsNullOrWhiteSpace(result.BackIcon))
                {
                    result.BackIcon = "";
                }

                result.CommandScope = commandScope;
                return result;
            });
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync(nameof(TapGestureRecognizer_Tapped), async delegate ()
            {
                View view = (sender as View);
                IDataViewItem dataViewItem = view.BindingContext as IDataViewItem;
                if (dataViewItem != null)
                {
                    PreparedData preparedData = dataViewItem.PreparedData as PreparedData;
                    if(preparedData != null)
                    {
                        if (preparedData?.CommandScope?.CommandProcessor != null)
                        {
                            await preparedData.CommandScope.CommandProcessor.ExecuteCommandAsync(preparedData.CommandScope, preparedData.CommandName, preparedData.CommandParameter);
                        }
                    }
                }
            });
        }

        public class PreparedData : PropertyClass
        {
            private string _textColor;
            public string TextColor
            {
                get { return _textColor; }
                set { SetProperty(ref _textColor, value); }
            }

            private string _backgroundColor;
            public string BackgroundColor
            {
                get { return _backgroundColor; }
                set { SetProperty(ref _backgroundColor, value); }
            }

            private string _backIcon;
            public string BackIcon
            {
                get { return _backIcon; }
                set { SetProperty(ref _backIcon, value); }
            }

            private string _title;
            public string Title
            {
                get { return _title; }
                set { SetProperty(ref _title, value); }
            }

            public string CommandName { get; set; }
            public string CommandParameter { get; set; }

            [JsonIgnore]
            public ICommandScope CommandScope { get; set; }
        }


    }
}