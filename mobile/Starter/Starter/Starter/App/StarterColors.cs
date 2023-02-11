using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.App
{
    public class StarterColors
    {
        static StarterColors()
        {
            Current = LIGHT;
        }
        public static StarterColors Current { get; set; }

        public static readonly StarterColors LIGHT = new StarterColors()
        {
            InputPromptBackground = "#333333",
            InputPromptForeground = "#ffffff",
            InputPlaceholder = "#b1b1b1",
            HeaderAttention = "#ffffa300",
            ButtonAttention = "#fffe6f06",
            TextOnAttention = "#000000",
            TextColor = "#000000",
            BorderColor = "#000000",
            PrimaryAccent = "#b10101",
            PrimaryAccentShadow = "#530101",
            TextColorFaded = "#a1a1a1",
            ButtonBackground = "#fafafa",
            ButtonBackgroundFaded = "#a6a5a5",
            PrimaryBackground = "#ededed",
        };

        public static readonly StarterColors DARK = new StarterColors()
        {
            InputPromptBackground = "#f6f6f6",
            InputPromptForeground = "#000000",
            InputPlaceholder = "#666666",
            HeaderAttention = "#fdc202",
            ButtonAttention = "#fdc202",
            TextOnAttention = "#000000",
            TextColor = "#ffffff",
            BorderColor = "#ffffff",
            PrimaryAccent = "#b10101",
            PrimaryAccentShadow = "#530101",
            TextColorFaded = "#cccccc",
            ButtonBackground = "#333333",
            ButtonBackgroundFaded = "#666666",
            PrimaryBackground = "#999999",
        };


        public string InputPromptBackground { get; protected set; }
        public string InputPromptForeground { get; protected set; }
        public string InputPlaceholder { get; protected set; }


        public string HeaderAttention { get; protected set; }
        public string TextOnAttention { get; protected set; }
        public string ButtonAttention { get; protected set; }

        public string PrimaryAccent { get; protected set; }
        public string PrimaryAccentShadow { get; protected set; }

        public string TextColor { get; protected set; }
        public string BorderColor { get; protected set; }
        public string TextColorFaded { get; protected set; }
        public string ButtonBackground { get; protected set; }
        public string ButtonBackgroundFaded { get; protected set; }
        public string PrimaryBackground { get; protected set; }

    }

    public class ColorPair
    {
        public string Empty { get; set; }
        public string Full { get; set; }
    }
}
