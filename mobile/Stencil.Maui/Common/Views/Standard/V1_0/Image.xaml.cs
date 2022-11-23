using Newtonsoft.Json;
using Stencil.Maui.Commanding;
using Stencil.Maui.Platform;
using Stencil.Maui.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;

namespace Stencil.Maui.Views.Standard.v1_0
{
    public partial class Image : ResourceDictionary, IDataViewComponent
    {
        public Image()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "image";

        private const string TEMPLATE_KEY = "image";
        
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
                return this[TEMPLATE_KEY] as DataTemplate;
            });
        }
        public Task<IDataViewItemReference> PrepareBindingContextAsync(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json)
        {
            return CoreUtility.ExecuteFunction($"{COMPONENT_NAME}.{nameof(PrepareBindingContextAsync)}", delegate ()
            {
                ImageContext result = null;

                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<ImageContext>(configuration_json);
                }
                if(result == null)
                {
                    result = new ImageContext();
                }
                
                if(result.FullBleedHorizontal && result.ImageWidth > 0 && result.ImageHeight > 0)
                {
                    result.UISource = null;
                    result.Width = -1; // computed when grid is available
                    result.Height = -1; // computed when grid is available
                }
                else
                {
                    result.UISource = result.Source;
                }
                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return Task.FromResult<IDataViewItemReference>(result);
            });
        }

        /// <summary>
        /// This is not a mistake, images do not always render correct sizing when image cache is enabled. [and we really want image caching]
        /// </summary>
        private void AssignImageSize_BugFix(StackLayout layout)
        {
            CoreUtility.ExecuteMethod(nameof(AssignImageSize_BugFix), delegate ()
            {
                if (layout?.Width > 0)
                {
                    ImageContext context = layout.BindingContext as ImageContext;
                    if (context != null)
                    {
                        if (context.FullBleedHorizontal && context.ImageWidth > 0 && context.ImageHeight > 0)
                        {
                            context.Width = (int)layout.Width;
                            context.Height = (int)(layout.Width * (float)context.ImageHeight / (float)context.ImageWidth);// W/H = w/?;

                            context.UISource = context.Source;
                            //layout.ForceLayout();  //TODO:MAUI: No longer available, is this still required?
                        }
                    }
                }
            });
        }

        /// <summary>
        /// This is not a mistake, images do not always render correct sizing when cache is enabled.
        /// This forced a layout call
        /// </summary>
        private void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CoreUtility.ExecuteMethod(nameof(StackLayout_PropertyChanged), delegate ()
            {
                if (e.PropertyName == StackLayout.WidthProperty.PropertyName)
                {
                    StackLayout layout = (sender as StackLayout);
                    if (layout?.Width > 0)
                    {
                        MainThread.BeginInvokeOnMainThread(delegate ()
                        {
                            this.AssignImageSize_BugFix(layout);
                        });
                    }
                }
            });
        }

        private async void Image_Tapped(object sender, EventArgs e)
        {
            await CoreUtility.ExecuteMethodAsync($"{COMPONENT_NAME}.Image_Tapped", async delegate ()
            {
                View view = (sender as View);
                ImageContext context = view?.BindingContext as ImageContext;
                if (context != null)
                {
                    NativeApplication.Keyboard?.TryHideKeyboard();

                    if (!string.IsNullOrWhiteSpace(context.CommandName))
                    {
                        if (context.CommandScope?.CommandProcessor != null)
                        {
                            await context.CommandScope.CommandProcessor.ExecuteCommandAsync(context.CommandScope, context.CommandName, context.CommandParameter, context?.DataViewItem?.DataViewModel);
                        }
                    }
                }
            });
        }
    }

    public class ImageContext : PreparedBindingContext
    {
        public ImageContext()
            : base(nameof(ImageContext))
        {

        }

        public string CommandName { get; set; }
        public string CommandParameter { get; set; }

        private bool _fullBleedHorizontal;
        public bool FullBleedHorizontal
        {
            get { return _fullBleedHorizontal; }
            set { SetProperty(ref _fullBleedHorizontal, value); }
        }

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


        private int _imageWidth = -1;
        public int ImageWidth
        {
            get { return _imageWidth; }
            set { SetProperty(ref _imageWidth, value); }
        }

        private int _imageHeight = -1;
        public int ImageHeight
        {
            get { return _imageHeight; }
            set { SetProperty(ref _imageHeight, value); }
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


        private ImageSource _uiSource;
        public ImageSource UISource
        {
            get { return _uiSource; }
            set { SetProperty(ref _uiSource, value); }
        }
    }
}