using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Stencil.Maui.Commanding;
using Stencil.Maui.Views.Markdown;
using Stencil.Maui.Views.Standard;
using Stencil.Maui.Views.Standard.v1_0;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Stencil.Maui.Views
{
    public class ComponentLibraryTemplateSelector : DataTemplateSelector, IResolvableTemplateSelector
    {
        #region Constructors

        static ComponentLibraryTemplateSelector()
        {
            ConcurrentDictionary<string, List<IComponentLibrary>> libraries = new ConcurrentDictionary<string, List<IComponentLibrary>>();
            libraries[StandardLibrary.LIBRARY_NAME] = new List<IComponentLibrary>() // order matters, first match wins
            {
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
                        label.BackgroundColor = Colors.Red;
                        label.TextColor = Colors.White;
                        label.SetBinding(Label.TextProperty, $"{nameof(IDataViewItem.PreparedContext)}.TypeName");
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
                                if (dataViewItem.PreparedContext == null || !dataViewComponent.BindingContextCacheEnabled)
                                {
#pragma warning disable 0618
                                    dataViewItem.PreparedContext = dataViewComponent.PrepareBindingContextAsync(this.CommandScope, dataViewItem.DataViewModel, dataViewItem, this, dataViewItem.ConfigurationJson).SyncResult();
#pragma warning restore
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

                if(dataViewItem.UIDataTemplate != null)
                {
                    if(dataViewItem.UIDataTemplate.TryGetTarget(out DataTemplate found) && found != null)
                    {
                        return found;
                    }
                }

                IDataViewComponent dataViewComponent = this.ResolveTemplateAndPrepareData(dataViewItem);
                if (dataViewComponent != null)
                {
                    DataTemplate result = dataViewComponent.GetDataTemplate();
                    dataViewItem.UIDataTemplate = new WeakReference<DataTemplate>(result);
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
                CoreUtility.Logger.LogError("ComponentLibraryTemplateSelector", new Exception("Unable to find component with the name " + componentName));

                // return empty
                return this.MissingTemplate;
            });
        }
    }
}
