﻿
using System;
using Xamarin.Forms;

namespace Stencil.Forms.Views
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
        /// Warning: Does not support property changed notification, rely on custom application of changes instead
        /// </summary>
        IDataViewModel DataViewModel { get; set; }

        /// <summary>
        /// Child dataview elements for the current view
        /// </summary>
        IDataViewSection[] Sections { get; set; }

        /// <summary>
        /// Nested elemnts wrapped in a layout
        /// Warning: Does not support property changed notification, rely on custom application of changes instead
        /// </summary>
        IDataViewItem[] EncapsulatedItems { get; set; }

        /// <summary>
        /// Optional filter for the view
        /// Warning: Does not support property changed notification, rely on custom application of changes instead
        /// </summary>
        IDataViewFilter ViewFilter { get; }

        WeakReference<DataTemplate> UIDataTemplate { get; set; }

    }
}
