
namespace Stencil.Native.Views
{
    public interface IDataViewItem
    {
        /// <summary>
        /// The library containing the component
        /// </summary>
        string Library { get; }
        
        /// <summary>
        /// Name of the component to load
        /// </summary>
        string Component { get; }

        /// <summary>
        /// Configuration given to the view component
        /// </summary>
        string ConfigurationJson { get; }
        
        /// <summary>
        /// Dynamically generated BindingContext used to bind against the current component
        /// </summary>
        object PreparedContext { get; set; }

        /// <summary>
        /// The current viewmodel driving the view that contains this component
        /// </summary>
        IDataViewModel DataViewModel { get; set; }

        /// <summary>
        /// Child dataview elements for the current view
        /// </summary>
        IDataViewSection[] Sections { get; set; }

    }
}
