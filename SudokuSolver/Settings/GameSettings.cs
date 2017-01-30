using System;

namespace SudokuSolver.Settings
{
    public class GameSettings
    {
        public static int colSize = 9;
        public static int rowSize = 9;
        public static int gridSize = 3;
        public static int minValue = 1;
        public static int maxValue = 9;

        public const int maxColSize = 12;
        public const int maxRowSize = 12;
        public const int maxGridSize = 12;

        public static bool isRunning = false;
    }
}