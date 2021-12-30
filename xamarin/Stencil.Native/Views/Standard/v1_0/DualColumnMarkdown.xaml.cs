﻿using Newtonsoft.Json;
using Stencil.Native.Commanding;
using Stencil.Native.Markdown;
using Stencil.Native.Resourcing;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Stencil.Native.Views.Standard.v1_0
{
    public partial class DualColumnMarkdown : ResourceDictionary, IDataViewComponent
    {
        public DualColumnMarkdown()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "dualColumnMarkdown-" + StandardComponentsV1_0.NAME;

        private const string TEMPLATE_KEY = "dualColumnMarkdown";

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
                result.LinkTappedCommand = new Command<string>(async (destination) => await NativeApplication.CommandProcessor.LinkTapped(destination));
                result.CommandScope = commandScope;
                return result;
            });
        }
        public class PreparedData : PropertyClass
        {
            private string _backgroundColor;
            public string BackgroundColor
            {
                get { return _backgroundColor; }
                set { SetProperty(ref _backgroundColor, value); }
            }

            private List<MarkdownSection> _markdownLeft;
            public List<MarkdownSection> MarkdownLeft
            {
                get { return _markdownLeft; }
                set { SetProperty(ref _markdownLeft, value); }
            }

            private List<MarkdownSection> _markdownRight;
            public List<MarkdownSection> MarkdownRight
            {
                get { return _markdownRight; }
                set { SetProperty(ref _markdownRight, value); }
            }

            private bool _suppressDivider;
            public bool SuppressDivider
            {
                get { return _suppressDivider; }
                set { SetProperty(ref _suppressDivider, value); }
            }

            private int _fontSize = 16;
            public int FontSize
            {
                get { return _fontSize; }
                set { SetProperty(ref _fontSize, value); }
            }

            private string _textColor;
            public string TextColor
            {
                get { return _textColor; }
                set { SetProperty(ref _textColor, value); }
            }

            private Thickness _padding;
            public Thickness Padding
            {
                get { return _padding; }
                set { SetProperty(ref _padding, value); }
            }

            [JsonIgnore]
            public ICommand LinkTappedCommand { get; set; }
            [JsonIgnore]
            public ICommandScope CommandScope { get; set; }
        }
    }
}