using Microsoft.Maui.Controls;
using Stencil.Maui.Commanding;
using System.Threading.Tasks;

namespace Stencil.Maui.Views
{
    public interface IDataViewComponent
    {
        /// <summary>
        /// Whether the template selector can cache the bindingcontext during initialization.
        /// Typically true unless a special situation is encountered.
        /// </summary>
        bool BindingContextCacheEnabled { get; }
        
        /// <summary>
        /// Gets the data template to use for this component
        /// </summary>
        /// <returns></returns>
        DataTemplate GetDataTemplate();

        /// <summary>
        /// Creates a binding context to use to bind against the datatemplate created for this component.
        /// </summary>
        /// <param name="commandScope">The current command scope for any command invocations that may need invoking</param>
        /// <param name="dataViewModel">The current view model that drives the view that contains this component</param>
        /// <param name="dataViewItem">The current container that contains the reference to the component and its binding context.</param>
        /// <param name="selector">The DataTemplateSelector that generated this component.</param>
        /// <param name="configuration_json">Remote/Local configuration requested for this component</param>
        /// <returns></returns>
        Task<IDataViewItemReference> PrepareBindingContextAsync(ICommandScope commandScope, IDataViewModel dataViewModel, IDataViewItem dataViewItem, DataTemplateSelector selector, string configuration_json);
    }
}
