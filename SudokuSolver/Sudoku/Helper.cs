using System;
using System.Collections.Generic;

using SudokuSolver.Settings;

namespace SudokuSolver.Sudoku
{
    class Helper
    {
        /// <summary>
        /// Checks if the given sudoku board is valid(does not break any rules)
        /// getAll true | Will return a list of ALL invalid pieces/tiles/numbers
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public List<Sudoku> validBoard(int[,] boardState, bool getAll)
        {
            List<Sudoku> invalidPos = new List<Sudoku>();

            invalidPos.AddRange(checkCol(boardState, getAll));
            if (invalidPos.Count > 0 && !getAll)
                return invalidPos;
            
            invalidPos.AddRange(checkRow(boardState, getAll));
            if (invalidPos.Count > 0 && !getAll)
                return invalidPos;
            
            invalidPos.AddRange(checkGrid(boardState, getAll));

            return invalidPos;
        }

        /// <summary>
        /// Checks if the columns on the given sudoku board is valid.
        /// getAll true | Will return a list of ALL invalid pieces/tiles/numbers
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        private List<Sudoku> checkCol(int[,] boardState, bool getAll)
        {
            List<Sudoku> invalid = new List<Sudoku>();
            for (int row = 0; row < GameSettings.rowSize; row++)
            {
                List<Sudoku> numbers = new List<Sudoku>();
                for (int col = 0; col < GameSettings.colSize; col++)
                {
                    int currNum = boardState[col, row];
                    if (currNum != 0)
                    {
                        foreach (Sudoku s in numbers)
                        {
                            if (s.value == currNum)
                            {
                                invalid.Add(s);
                                invalid.Add(new Sudoku(currNum, col, row));
                                if (!getAll)
                                    return invalid;
                            }
                        }
                        numbers.Add(new Sudoku(currNum, col, row));
                    }
                }
                numbers.Clear();
            }
            return invalid;
        }

        /// <summary>
        /// Checks if the rows on the given sudoku board is valid.
        /// getAll true | Will return a list of ALL invalid pieces/tiles/numbers
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        private List<Sudoku> checkRow(int[,] boardState, bool getAll)
        {
            List<Sudoku> invalid = new List<Sudoku>();
            for (int col = 0; col < GameSettings.colSize; col++)
            {
                List<Sudoku> numbers = new List<Sudoku>();
                for (int row = 0; row < GameSettings.rowSize; row++)
                {
                    int currNum = boardState[col, row];
                    if (currNum != 0)
                    {
                        foreach (Sudoku s in numbers)
                        {
                            if (s.value == currNum)
                            {
                                invalid.Add(s);
                                invalid.Add(new Sudoku(currNum, col, row));
                                if (!getAll)
                                    return invalid;
                            }
                        }
                        numbers.Add(new Sudoku(currNum, col, row));
                    }
                }
                numbers.Clear();
            }
            return invalid;
        }

        /// <summary>
        /// Checks if the grid on the given sudoku board is valid.
        /// getAll true | Will return a list of ALL invalid pieces/tiles/numbers
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        private List<Sudoku> checkGrid(int[,] boardState, bool getAll)
        {
            List<Sudoku> invalid = new List<Sudoku>();
            for (int colStart = 0; colStart <= GameSettings.colSize - GameSettings.gridSize; colStart += GameSettings.gridSize)
            {
                for (int rowStart = 0; rowStart <= GameSettings.rowSize - GameSettings.gridSize; rowStart += GameSettings.gridSize)
                {
                    List<Sudoku> numbers = new List<Sudoku>();
                    for (int c = 0; c < GameSettings.gridSize; c++)
                    {
                        for (int r = 0; r < GameSettings.gridSize; r++)
                        {
                            int col = c + colStart;
                            int row = r + rowStart;
                            int currNum = boardState[col, row];
                            if (currNum != 0)
                            {
                                foreach (Sudoku s in numbers)
                                {
                                    if (s.value == currNum)
                                    {
                                        invalid.Add(s);
                                        invalid.Add(new Sudoku(currNum, col, row));
                                        if (!getAll)
                                            return invalid;
                                    }
                                }
                                numbers.Add(new Sudoku(currNum, col, row));
                            }
                        }
                    }
                }
            }
            return invalid;
        }

        /// <summary>
        /// Adds a number to a tile on the Sudoku board.
        /// if the tile added breaks the rules after all tries, it's set to 0 and the main program starts to backtrace.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public bool addTile(int[,] board, int col, int row, int minValue)
        {
            for (int val = minValue; val <= GameSettings.maxValue; val++)
            {
                board[col, row] = val;
                List<Sudoku> s = validBoard(board, false);
                if (s.Count == 0)
                    return true;
            }
            board[col, row] = 0;
            return false;
        }

        /// <summary>
        /// Calculates the new value of col.
        /// </summary>
        /// <param name="up"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public int incrementCol(bool up, int col, int row)
        {
            if (up)
            {
                if (row == Settings.GameSettings.rowSize - 1)
                    return ++col;
            }
            else
            {
                if (row == 0)
                    return --col;
            }
            return col;
        }

        /// <summary>
        /// Calculates the new value of row.
        /// </summary>
        /// <param name="up"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public int incrementRow(bool up, int col, int row)
        {
            if (up)
            {
                if (row == Settings.GameSettings.rowSize - 1)
                    return 0;
                return ++row;
            }
            else
            {
                if (row == 0)
                    return Settings.GameSettings.rowSize - 1;
                return --row;
            }
        }
    }
}