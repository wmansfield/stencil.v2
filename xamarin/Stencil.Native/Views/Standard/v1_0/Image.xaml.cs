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
    public partial class Image : ResourceDictionary, IDataViewComponent
    {
        public Image()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "image-" + StandardComponentsV1_0.NAME;

        private const string TEMPLATE_KEY = "image";

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
            private string _source;
            public string Source
            {
                get { return _source; }
                set { SetProperty(ref _source, value); }
            }

            private int _width = -1;
            public int Width
            {
                get { return _width; }
                set { SetProperty(ref _width, value); }
            }

            private int _height = -1;
            public int Height
            {
                get { return _height; }
                set { SetProperty(ref _height, value); }
            }


            private int _mimimumWidth = -1;
            public int MinimumWidth
            {
                get { return _mimimumWidth; }
                set { SetProperty(ref _mimimumWidth, value); }
            }

            private int _minimumHeight = -1;
            public int MinimumHeight
            {
                get { return _minimumHeight; }
                set { SetProperty(ref _minimumHeight, value); }
            }

            private Thickness _padding = new Thickness();
            public Thickness Padding
            {
                get { return _padding; }
                set { SetProperty(ref _padding, value); }
            }

            private string _backgroundColor;
            public string BackgroundColor
            {
                get { return _backgroundColor; }
                set { SetProperty(ref _backgroundColor, value); }
            }
        }
    }
}