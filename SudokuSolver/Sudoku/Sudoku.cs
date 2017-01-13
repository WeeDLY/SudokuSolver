using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Sudoku
{
    /// <summary>
    /// This class holds 2 board position.
    /// board is the current board position, while boardSet is the board position with the pieces that are locked.
    /// </summary>
    public class Boards
    {
        public int[,] board;
        public int[,] boardSet;
        public Boards(int[,] board, int[,] boardSet)
        {
            this.board = board;
            this.boardSet = boardSet;
        }
    }

    /// <summary>
    /// Sudoku class, this class holds the column, row and the value of the sudoku tile
    /// </summary>
    public class Sudoku
    {
        public int value;
        public int col;
        public int row;
        public Sudoku(int value, int col, int row)
        {
            this.value = value;
            this.col = col;
            this.row = row;
        }

        public static bool operator ==(Sudoku s1, Sudoku s2)
        {
            if(s1.col == s2.col && s1.row == s2.row)
                return true;
            else
                return false;
        }

        public static bool operator !=(Sudoku s1, Sudoku s2)
        {
            if(s1.col != s2.col || s1.row != s2.row)
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}