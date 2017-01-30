using System;
using System.ComponentModel;
using System.Collections.Generic;

using SudokuSolver.Settings;

namespace SudokuSolver.Sudoku
{
    class Solver
    {
        private MainWindow main;
        private int[,] board;
        private int[,] boardSet;

        private Helper helper = new Helper();

        /// <summary>
        /// SudokuSolver.Solver constructor.
        /// This takes the GUI it leaves feedback to, a board position and a board position with the pieces not allowed to change.(Set tiles/numbers)
        /// </summary>
        /// <param name="main"></param>
        /// <param name="board"></param>
        /// <param name="boardSet"></param>
        public Solver(MainWindow main, int[,] board, int[,] boardSet)
        {
            this.main = main;
            this.board = board;
            this.boardSet = boardSet;
        }

        /// <summary>
        /// Check if the sudoku board position is valid.
        /// If it's valid, it creates a thread that solves the board position given.
        /// else it displays the wrongly position tiles/numbers.
        /// </summary>
        public void StartSolver()
        {
            List<Sudoku> invalid = helper.validBoard(board, true);
            if(invalid.Count == 0)
            {
                GameSettings.isRunning = true;

                BackgroundWorker bw = new BackgroundWorker();
                bw.RunWorkerAsync();
                bw.DoWork += bw_DoWork;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            }
            else
            {
                foreach(Sudoku s in invalid)
                {
                    main.TextColor(s.value, s.col, s.row);
                }
                Console.WriteLine("not valid board position");
                main.TextReadOnly(false);
            }
        }

        /// <summary>
        /// Event that is called from the StartSolver thread, when the sudoku puzzle is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GameSettings.isRunning = false;
            main.Draw(board, boardSet);
        }

        /// <summary>
        /// This is the main function that solves the given sudoku puzzle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int highest = 0;
            int col = 0, row = 0;
            bool force = false;
            while (col < Settings.GameSettings.colSize && row < Settings.GameSettings.rowSize)
            {
                int curNum = board[col, row];
                if ((curNum == 0 || force) && boardSet[col, row] == 0)
                {
                    curNum = curNum == 0 ? GameSettings.minValue : ++curNum;

                    bool added = helper.addTile(board, col, row, curNum);
                    if (!added)
                        force = true;
                    else
                        force = false;
                }

                col = helper.incrementCol(!force, col, row);
                row = helper.incrementRow(!force, col, row);
                int progress = col * 10 + row;
                if (progress > highest)
                {
                    highest = progress;
                    main.progressBar.Dispatcher.Invoke(new UpdateProgressCallback(this.UpdateProgress), progress);
                    //main.Dispatcher.Invoke(new UpdateDraw(main.Draw), board, boardSet);
                }
            }
            Console.WriteLine("done");
        }
        //private delegate void UpdateDraw(int[,] board, int[,] boardSet);

        private delegate void UpdateProgressCallback(int val);
        /// <summary>
        /// This is the function that updates the progressBar for the sudoku solving.
        /// This function is ONLY called by a delegate for async purposes.
        /// </summary>
        /// <param name="val"></param>
        private void UpdateProgress(int val)
        {
            main.progressBar.Value = val;
        }
    }
}