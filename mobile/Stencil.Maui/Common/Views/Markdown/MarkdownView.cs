using Microsoft.Maui.Controls;
using Stencil.Common.Markdown;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Stencil.Maui.Views
{
    public class MarkdownView : View, IMarkdownHost
    {
        public MarkdownView()
        {
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(propertyName: nameof(TextColor), returnType: typeof(string), declaringType: typeof(MarkdownView), defaultValue: null);
        public string TextColor
        {
            get { return (string)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty SectionsProperty = BindableProperty.Create(propertyName: nameof(Sections),returnType: typeof(List<MarkdownSection>),declaringType: typeof(MarkdownView),defaultValue: null);
        public List<MarkdownSection> Sections
        {
            get { return (List<MarkdownSection>)GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        public static readonly BindableProperty SuppressDividerProperty = BindableProperty.Create(propertyName: nameof(SuppressDivider),returnType: typeof(bool),declaringType: typeof(MarkdownView),defaultValue: false);
        public bool SuppressDivider
        {
            get { return (bool)GetValue(SuppressDividerProperty); }
            set { SetValue(SuppressDividerProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(propertyName: nameof(FontSize), returnType: typeof(int), declaringType: typeof(MarkdownView), defaultValue: 16);
        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty LinkTappedCommandProperty = BindableProperty.Create(propertyName: nameof(LinkTappedCommand),returnType: typeof(ICommand),declaringType: typeof(MarkdownView),defaultValue: null);
        public ICommand LinkTappedCommand
        {
            get { return (Command)GetValue(LinkTappedCommandProperty); }
            set { SetValue(LinkTappedCommandProperty, value); }
        }


        public static readonly BindableProperty AnythingTappedCommandProperty = BindableProperty.Create(propertyName: nameof(AnythingTappedCommand), returnType: typeof(ICommand),declaringType: typeof(MarkdownView),defaultValue: null);
        public ICommand AnythingTappedCommand
        {
            get { return (Command)GetValue(AnythingTappedCommandProperty); }
            set { SetValue(AnythingTappedCommandProperty, value); }
        }

        public void LinkTapped(string agument)
        {
            CoreUtility.ExecuteMethod(nameof(LinkTapped), delegate ()
            {
                this.LinkTappedCommand?.Execute(agument);
            });
        }

        public void AnythingTapped()
        {
            CoreUtility.ExecuteMethod(nameof(AnythingTapped), delegate ()
            {
                this.AnythingTappedCommand?.Execute(null);
            });

        }

        
    }
}
