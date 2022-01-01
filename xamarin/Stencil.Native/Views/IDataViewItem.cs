
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
        /// Typed object used by the view component
        /// </summary>
        object PreparedData { get; set; }

        IDataViewModel DataViewModel { get; set; }
        IDataViewSection[] Sections { get; set; }

    }
}
