using Stencil.Native.Commanding;
using Stencil.Native.Views.Markdown;
using Stencil.Native.Views.Standard;
using Stencil.Native.Views.Standard.v1_0;
using Stencil.Native.Views.Standard.v1_1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Stencil.Native.Views
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
                        label.SetBinding(Label.TextProperty, "PreparedData");
#endif
                        return label;
                    });
                }
                return _missingTemplate;
            }
        }

#endregion

        public IDataViewComponent ResolveTemplateAndPrepareData(IDataViewItem dataViewItem)
        {
            return CoreUtility.ExecuteFunction(nameof(ResolveTemplateAndPrepareData), delegate ()
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
                                if (dataViewItem.PreparedData == null || dataViewComponent.PreparedDataCacheDisabled)
                                {
                                    dataViewItem.PreparedData = dataViewComponent.PrepareData(this.CommandScope, dataViewItem.DataViewModel, dataViewItem, this, dataViewItem.ConfigurationJson);
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

                IDataViewComponent dataViewComponent = this.ResolveTemplateAndPrepareData(dataViewItem);

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
                dataViewItem.PreparedData = componentName;
#if !DEBUG
                CoreUtility.Logger.LogError("ComponentLibraryTemplateSelector", new Exception("Unable to find component with the name " + componentName));
#endif

                // return empty
                return this.MissingTemplate;
            });
        }
    }
}
