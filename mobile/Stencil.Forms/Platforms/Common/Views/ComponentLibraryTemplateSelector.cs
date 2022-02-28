using Stencil.Forms.Commanding;
using Stencil.Forms.Views.Markdown;
using Stencil.Forms.Views.Standard;
using Stencil.Forms.Views.Standard.v1_0;
using Stencil.Forms.Views.Standard.v1_1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms.Views
{
    public class ComponentLibraryTemplateSelector : DataTemplateSelector, IResolvableTemplateSelector
    {
        #region Constructors

        static ComponentLibraryTemplateSelector()
        {
            ConcurrentDictionary<string, List<IComponentLibrary>> libraries = new ConcurrentDictionary<string, List<IComponentLibrary>>();
            libraries[StandardLibrary.LIBRARY_NAME] = new List<IComponentLibrary>() // order matters, first match wins
            {
                new StandardComponentsV1_1(),
                new StandardComponentsV1_0(),
                new MarkdownComponentsV1_0()
            };
            libraries[string.Empty] = libraries[StandardLibrary.LIBRARY_NAME];

            ComponentLibraries = libraries;
        }

        public ComponentLibraryTemplateSelector(ICommandScope commandScope)
        {
            this.CommandScope = commandScope;
        }

        #endregion

        #region Properties

        public static ConcurrentDictionary<string, List<IComponentLibrary>> ComponentLibraries;

        public ICommandScope CommandScope { get; set; }


        private DataTemplate _missingTemplate;
        protected virtual DataTemplate MissingTemplate
        {
            get
            {
                if(_missingTemplate == null)
                {
                    _missingTemplate = new DataTemplate(() => 
                    {
                        Label label = new Label();
#if DEBUG
                        label.BackgroundColor = Color.Red;
                        label.TextColor = Color.White;
                        label.SetBinding(Label.TextProperty, nameof(IDataViewItem.PreparedContext));
#endif
                        return label;
                    });
                }
                return _missingTemplate;
            }
        }

#endregion

        public Task<IDataViewComponent> ResolveTemplateAndPrepareDataAsync(IDataViewItem dataViewItem)
        {
            return CoreUtility.ExecuteFunctionAsync(nameof(ResolveTemplateAndPrepareDataAsync), async delegate ()
            {
                if (dataViewItem != null && !string.IsNullOrWhiteSpace(dataViewItem.Component))
                {
                    string library = dataViewItem.Library;
                    if (library == null)
                    {
                        library = string.Empty;
                    }
                    if (ComponentLibraries.TryGetValue(library, out List<IComponentLibrary> componentLibraries))
                    {
                        foreach (IComponentLibrary componentLibrary in componentLibraries)
                        {
                            IDataViewComponent dataViewComponent = componentLibrary.GetComponent(dataViewItem.Component);
                            if (dataViewComponent != null)
                            {
                                if (dataViewItem.PreparedContext == null || !dataViewComponent.BindingContextCacheEnabled)
                                {
                                    dataViewItem.PreparedContext = await dataViewComponent.PrepareBindingContextAsync(this.CommandScope, dataViewItem.DataViewModel, dataViewItem, this, dataViewItem.ConfigurationJson);
                                }
                                return dataViewComponent;
                            }
                        }
                    }
                }
                return null;
            });
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return CoreUtility.ExecuteFunction(nameof(OnSelectTemplate), delegate ()
            {
                IDataViewItem dataViewItem = item as IDataViewItem;
                if (dataViewItem == null)
                {
                    IDataViewItemReference dataViewItemReference = item as IDataViewItemReference;
                    if(dataViewItemReference != null)
                    {
                        dataViewItem = dataViewItemReference.DataViewItem;
                    }
                }
                
                IDataViewComponent dataViewComponent = this.ResolveTemplateAndPrepareDataAsync(dataViewItem).Result; //TODO:SHOULD: Bad Async Result

                if (dataViewComponent != null)
                {
                    DataTemplate result = dataViewComponent.GetDataTemplate();
                    return result;
                }

                // log failure
                string componentName = dataViewItem?.Component;
                if(string.IsNullOrWhiteSpace(componentName) && item != null)
                {
                    componentName = item.GetType().ToString();
                }
                if (dataViewItem != null)
                {
                    dataViewItem.PreparedContext = componentName;
                }
#if !DEBUG
                CoreUtility.Logger.LogError("ComponentLibraryTemplateSelector", new Exception("Unable to find component with the name " + componentName));
#endif

                // return empty
                return this.MissingTemplate;
            });
        }
    }
}
