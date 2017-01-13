using System;
using System.Windows.Media;

namespace SudokuSolver.Settings
{
    public enum Language
    {
        English,
        Norsk,
        Español
    }

    class InterfaceSettings
    {
        public const int fontSize = 24;
        public const int textBoxSize = 32;

        public static readonly Brush[] colors = new Brush[] { Brushes.Red, Brushes.Yellow, Brushes.Green, Brushes.Violet };

        // The space between the different grids
        public const int marginSpace = 3;

        public static Language selectedLanguage = Language.English;
    }
}