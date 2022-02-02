using Newtonsoft.Json;
using Stencil.Common.Screens;
using Stencil.Forms.Commanding;
using Stencil.Forms.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard.v1_0
{
    public partial class Carousel : ResourceDictionary, IDataViewComponent
    {
        public Carousel()
        {
            InitializeComponent();
        }

        public const string COMPONENT_NAME = "carousel";

        public const string STATE_KEY_POSITION = "position";
        public const string STATE_KEY_PAGE = "page";
        public const string EMIT_KEY_MOVE = "move";

        private const string TEMPLATE_KEY = "carousel";

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
            return CoreUtility.ExecuteFunctionAsync<IDataViewItemReference>($"{COMPONENT_NAME}.{nameof(PrepareBindingContextAsync)}", async delegate ()
            {
                CarouselContext result = null;
                if (!string.IsNullOrWhiteSpace(configuration_json))
                {
                    result = JsonConvert.DeserializeObject<CarouselContext>(configuration_json);
                }
                if (result == null)
                {
                    result = new CarouselContext();
                }

                List<INestedDataViewModel> cells = new List<INestedDataViewModel>();

                if (dataViewItem?.Sections != null)
                {
                    foreach (IDataViewSection section in dataViewItem.Sections)
                    {
                        if (section.ViewItems?.Length > 0)
                        {
                            StandardNestedDataViewModel childViewModel = new StandardNestedDataViewModel(commandScope.CommandProcessor, selector)
                            {
                                MainItemsUnFiltered = new ObservableCollection<IDataViewItem>(section.ViewItems),
                                MainItemsFiltered = new ObservableCollection<object>(section.ViewItems.Select(x => x.PreparedContext))
                            };

                            // extract filters or augmentations
                            await childViewModel.ExtractAndPrepareExtensionsAsync();

                            // apply page visuals
                            if (section.VisualConfig != null)
                            {
                                childViewModel.BackgroundImage = section.VisualConfig.BackgroundImage;

                                if (!string.IsNullOrWhiteSpace(section.VisualConfig.BackgroundColor))
                                {
                                    childViewModel.BackgroundColor = Color.FromHex(section.VisualConfig.BackgroundColor);
                                }
                                childViewModel.Padding = section.VisualConfig.Padding.ToThickness();
                            }

                            await childViewModel.Initialize();

                            cells.Add(childViewModel);
                        }
                    }
                }
                result.Cells = cells.ToArray();
                result.DataTemplateSelector = selector;

                result.CommandScope = commandScope;
                result.DataViewItem = dataViewItem;

                return result;
            });
        }

        private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            CoreUtility.ExecuteMethod($"{COMPONENT_NAME}.CarouselView_PositionChanged", delegate ()
            {
                View view = (sender as View);
                CarouselContext context = view?.BindingContext as CarouselContext;
                if (!string.IsNullOrEmpty(context.InteractionGroup))
                {
                    IDataViewModel viewModel = context?.DataViewItem?.DataViewModel;

                    if (viewModel != null)
                    {
                        viewModel.RaiseStateChange(context.InteractionGroup, STATE_KEY_POSITION, e.CurrentPosition.ToString());
                        viewModel.RaiseStateChange(context.InteractionGroup, STATE_KEY_PAGE, (e.CurrentPosition + 1).ToString());
                    }
                }
            });
        }
    }

    

    public class CarouselContext : PreparedBindingContext, IStateEmitter
    {
        public CarouselContext()
            : base(nameof(CarouselContext))
        {
            this.HeightRequest = -1;
            this.VerticalOptions = LayoutOptions.Fill;
        }

        public string InteractionGroup { get; set; }

        public Thickness Padding { get; set; }
        public Thickness Margin { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }

        public double HeightRequest { get; set; }
        public LayoutOptions VerticalOptions { get; set; }

        private int _position;
        public int Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        private bool _loop;
        public bool Loop
        {
            get { return _loop; }
            set { SetProperty(ref _loop, value); }
        }

        private bool _bounceEnabled;
        public bool BounceEnabled
        {
            get { return _bounceEnabled; }
            set { SetProperty(ref _bounceEnabled, value); }
        }

        public string OverMoveCommandName { get; set; }
        public string OverMoveCommandParameter { get; set; }

        public string UnderMoveCommandName { get; set; }
        public string UnderMoveCommandParameter { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public DataTemplateSelector DataTemplateSelector { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public INestedDataViewModel[] Cells { get; set; }

        public Task ChangeStateAsync(string name, string state)
        {
            return base.ExecuteFunction(nameof(ChangeStateAsync), async delegate ()
            {
                if(name == null) { return; }

                switch (name)
                {
                    case Carousel.EMIT_KEY_MOVE:
                        switch (state)
                        {
                            case "-1":
                                if(this.Position > 0)
                                {
                                    this.Position = this.Position - 1;
                                }
                                else
                                {
                                    await this.RunCommandAsync(this.OverMoveCommandName, this.OverMoveCommandParameter);
                                }
                                break;
                            case "1":
                                if (this.Position + 1 < this.Cells.Length)
                                {
                                    this.Position = this.Position + 1;
                                }
                                else
                                {
                                    await this.RunCommandAsync(this.UnderMoveCommandName, this.UnderMoveCommandParameter);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            });
        }

        protected Task RunCommandAsync(string commandName, string commandParameter)
        {
            return base.ExecuteMethodAsync(nameof(RunCommandAsync), async delegate ()
            {
                if (!string.IsNullOrWhiteSpace(commandName))
                {
                    if (this.CommandScope?.CommandProcessor != null)
                    {
                        await this.CommandScope.CommandProcessor.ExecuteCommandAsync(this.CommandScope, commandName, commandParameter, this?.DataViewItem?.DataViewModel);
                    }
                }
            });
        }

        public void EmitDefaultState(INestedDataViewModel viewModel)
        {
            base.ExecuteMethod(nameof(EmitDefaultState), delegate ()
            {
                viewModel?.RaiseStateChange(this.InteractionGroup, Carousel.STATE_KEY_POSITION, 0.ToString());
                viewModel?.RaiseStateChange(this.InteractionGroup, Carousel.STATE_KEY_PAGE, 1.ToString());
            });
        }
    }

}