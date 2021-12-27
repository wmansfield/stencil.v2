using Stencil.Native.Commanding;
using Stencil.Native.Views.Markdown;
using Stencil.Native.Views.Standard;
using Stencil.Native.Views.Standard.v1_0;
using Stencil.Native.Views.Standard.v1_1;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Stencil.Native.Views
{
    public class ComponentLibraryTemplateSelector : DataTemplateSelector
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

        #endregion


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            IDataViewItem dataViewItem = item as IDataViewItem;
            string library = dataViewItem.Library;
            if (library == null)
            {
                library = string.Empty;
            }
            if(dataViewItem != null && !string.IsNullOrWhiteSpace(dataViewItem.Component))
            {
                if(ComponentLibraries.TryGetValue(library, out List<IComponentLibrary> componentLibraries))
                {
                    foreach (IComponentLibrary componentLibrary in componentLibraries)
                    {
                        IDataViewComponent dataViewComponent = componentLibrary.GetComponent(dataViewItem.Component);
                        if (dataViewComponent != null)
                        {
                            dataViewItem.PreparedData = dataViewComponent.PrepareData(this.CommandScope, this, dataViewItem.ConfigurationJson, dataViewItem.Sections);
                            return dataViewComponent.GetDataTemplate();
                        }
                    }
                }
            }
            return null;//TODO:MUST: Figure out a graceful way to fail here
        }
    }
}
